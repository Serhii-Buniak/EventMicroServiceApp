using AutoMapper;
using IdentityMicroService.BLL.Clients.Http;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Dtos;
using IdentityMicroService.BLL.Exceptions;
using IdentityMicroService.BLL.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using static IdentityMicroService.BLL.Constants.AuthorizationConfigs;

namespace IdentityMicroService.BLL.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwt;
    private readonly IImageClient _client;
    private readonly IMapper _mapper;

    public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwt, ApplicationDbContext context, IImageClient client, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _jwt = jwt.Value;
        _client = client;
        _mapper = mapper;
    }

    public async Task RegisterAsync(RegisterDto model)
    {
        var user = _mapper.Map<ApplicationUser>(model);

        if (model.CityId != null)
        {
            City? city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == model.CityId);

            if (city == null)
            {
                throw new NotFoundException(nameof(City), model.CityId);
            }
            user.City = city;
        }

        if (model.Image != null)
        {
            Image? image = await _client.CreateImage(model.Image);

            if (image == null)
            {
                throw new ClientException(nameof(IImageClient));
            }
            user.Image = image;
        }
        
        IdentityResult createUserResult = await _userManager.CreateAsync(user, model.Password);
        if (!createUserResult.Succeeded)
        {
            throw new ValidationModelException(createUserResult.Errors);
        }

        IdentityResult addRoleResult = await _userManager.AddToRoleAsync(user, roleDict[Roles.User]);
        if (!addRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            throw new ValidationModelException(addRoleResult.Errors);
        }
    }

    public async Task<AuthTokensDto> LogInAsync(LogInDto model, string? currRefreshToken)
    {
        AuthTokensDto authenticationModel = new();

        ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), model.Email);
        }

        bool canLogIn = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!canLogIn)
        {
            throw new AuthException("Uncorrect password");
        }

        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
        authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        if (model.RemeberMe)
        {
            var refreshToken = RefreshToken.Create();
            authenticationModel.RefreshToken = new RefreshTokenDto
            {
                Token = refreshToken.Token,
                Expiration = refreshToken.Expires
            };

            user.RefreshTokens.Add(refreshToken);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        else
        {
            if (currRefreshToken != null)
            {
                await RevokeToken(currRefreshToken);
            }
        }

        return authenticationModel;
    }

    public async Task<AuthTokensDto> RefreshTokenAsync(string token)
    {
        AuthTokensDto authTokensDto = new();

        ApplicationUser? user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), token);
        }

        RefreshToken refreshToken = user.RefreshTokens.First(x => x.Token == token);

        if (!refreshToken.IsActive)
        {
            throw new NotFoundException(nameof(RefreshToken), token);
        }

        refreshToken.Revoked = DateTime.UtcNow;

        var newRefreshToken = RefreshToken.Create();

        user.RefreshTokens.Add(newRefreshToken);
        _context.Update(user);
        await _context.SaveChangesAsync();

        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

        authTokensDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authTokensDto.RefreshToken = new RefreshTokenDto
        {
            Token = newRefreshToken.Token,
            Expiration = newRefreshToken.Expires
        };

        return authTokensDto;
    }
    public async Task RevokeToken(string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), token);
        }

        RefreshToken refreshToken = user.RefreshTokens.First(x => x.Token == token);
        if (!refreshToken.IsActive)
        {
            throw new NotFoundException(nameof(RefreshToken), token);
        }

        refreshToken.Revoked = DateTime.UtcNow;

        _context.Update(user);
        await _context.SaveChangesAsync();
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        }.Union(userClaims).Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken
        (
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials
        );

        return jwtSecurityToken;
    }
}
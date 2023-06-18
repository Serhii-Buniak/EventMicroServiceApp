using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Reflection;
using IdentityMicroService.BLL.Settings;
using IdentityMicroService.BLL.DAL.Data;
using IdentityMicroService.BLL.Services;
using IdentityMicroService.BLL.Constants;
using IdentityMicroService.BLL.Clients.Grpc;
using IdentityMicroService.BLL.Clients.Http;
using IdentityMicroService.BLL.Subscribers;
using IdentityMicroService.BLL.Subscribers.Processor;
using IdentityMicroService.WebApi;
using IdentityMicroService.WebApi.GrpcControllers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(AuthorizationConfigs)));

services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
    opt.User.RequireUniqueEmail= true;

    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;

}).AddEntityFrameworkStores<ApplicationDbContext>();

services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

services.AddHostedService<MassageBusSubscriber>();

services.AddSingleton<ICityEventProcessor, CityEventProcessor>();
services.AddSingleton<IImageEventProcessor, ImageEventProcessor>();

services.AddHttpClient<IImageClient, ImageClient>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IRoleService, RoleService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ICityClient, CityClient>();

services.AddGrpc();
services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


const string corsName = "identity-micro-service";
services.AddCors(options =>
{
    options.AddPolicy(corsName,
    corsbuilder =>
    {
        corsbuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
});


var app = builder.Build();

app.UseCors(corsName);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<UserGrpcController>();
app.MapGet("/protos/user.server.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("../IdentityMicroService.BLL/Protos/user.server.proto"));
});

PrepDb.PrepPopulation(builder);

app.Run();

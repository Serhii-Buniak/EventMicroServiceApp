using AutoMapper;
using Grpc.Core;
using IdentityMicroService.BLL.Dtos;
using IdentityMicroService.BLL.Protos;
using IdentityMicroService.BLL.Services;

namespace IdentityMicroService.WebApi.GrpcControllers;

public class UserGrpcController : GrpcUser.GrpcUserBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserGrpcController(IMapper mapper, IUserService userService)
	{
        _mapper = mapper;
        _userService = userService;
    }

    public override async Task<UsersResponse> GetAllUsers(GetAllUserRequest request, ServerCallContext context)
    {
        UsersResponse response = new();

        IEnumerable<UserDto> users = await _userService.GetAllAsync();
        var models = _mapper.Map<IEnumerable<GrpcUserModel>>(users);

        response.Users.AddRange(models);
        return response;
    }
}
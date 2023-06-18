using EventMicroService.Domain.Entities;

namespace EventMicroService.Application.Common.Interfaces
{
    public interface IUserClient
    {
        IEnumerable<User> GetAllUsers();
    }
}
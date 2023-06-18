using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Publishers;

public interface IMessageBusClient
{
    bool ConnectionIsOpen { get; }
    void SendMassage(ModelPublished modelPublished);
}
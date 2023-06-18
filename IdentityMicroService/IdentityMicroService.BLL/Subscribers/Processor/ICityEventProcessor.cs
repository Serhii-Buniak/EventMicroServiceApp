namespace IdentityMicroService.BLL.Subscribers.Processor;

public interface ICityEventProcessor
{
    void ProcessEvent(string message);
}
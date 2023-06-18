namespace IdentityMicroService.BLL.Subscribers.Processor
{
    public interface IImageEventProcessor
    {
        void ProcessEvent(string message);
    }
}
namespace IdentityMicroService.BLL.Subscribers.Subscribers;

public class ImageSubscribed : ModelPublished
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}

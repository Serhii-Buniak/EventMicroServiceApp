namespace IdentityMicroService.BLL.Subscribers.Subscribers;

public class CitySubscribed : ModelPublished
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long CountryId { get; set; }
}

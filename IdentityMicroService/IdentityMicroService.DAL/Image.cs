namespace IdentityMicroService.BLL.DAL.Data;

public class Image
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace IdentityMicroService.BLL.DAL.Data;

public class City
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long CountryId { get; set; }
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

}

using Microsoft.AspNetCore.Identity;

namespace IdentityMicroService.BLL.DAL.Data;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {

    }

    public ApplicationRole(string name) : base(name)
    {

    }
}

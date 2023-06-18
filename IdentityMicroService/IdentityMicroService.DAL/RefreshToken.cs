using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace IdentityMicroService.BLL.DAL.Data;

[Owned]
public class RefreshToken
{
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;

    public static RefreshToken Create()
    {
        var randomBytes = new byte[32];

        using RandomNumberGenerator numberGenerator = RandomNumberGenerator.Create();
        numberGenerator.GetBytes(randomBytes);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
    }

}

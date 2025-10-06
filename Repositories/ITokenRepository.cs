using Microsoft.AspNetCore.Identity;

namespace WebAPITemple.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}

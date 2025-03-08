using Backend.API.Models.Domain;

namespace Backend.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateToken(ApplicationUser user, List<string> roles);
    }
}

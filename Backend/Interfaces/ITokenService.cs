using Backend.Model;

namespace Backend.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}

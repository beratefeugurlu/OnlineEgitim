using OnlineEgitim.AdminAPI.Settings;

namespace OnlineEgitim.AdminAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username, string role);
    }
}

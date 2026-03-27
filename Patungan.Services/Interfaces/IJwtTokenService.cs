using Patungan.DataAccess.Entities;

namespace Patungan.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserModel user);
        string GenerateRefreshToken();
        int? ValidateToken(string token);
    }
}

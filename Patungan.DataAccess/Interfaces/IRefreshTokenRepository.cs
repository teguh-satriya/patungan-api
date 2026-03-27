using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenModel?> GetByTokenAsync(string token);
        Task<List<RefreshTokenModel>> GetActiveTokensByUserIdAsync(int userId);
        Task AddAsync(RefreshTokenModel refreshToken);
        Task UpdateAsync(RefreshTokenModel refreshToken);
        Task RevokeAllUserTokensAsync(int userId);
        Task RemoveExpiredTokensAsync();
    }
}

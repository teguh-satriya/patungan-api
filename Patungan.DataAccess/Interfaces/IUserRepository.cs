using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<UserModel?> GetByEmailAsync(string email);
        Task<UserModel?> GetByIdAsync(int id);
        Task AddUserAsync(UserModel user);
    }
}

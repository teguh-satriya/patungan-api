using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(UserModel user);
    }
}

using Microsoft.EntityFrameworkCore;
using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;

namespace Patungan.DataAccess.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly PatunganDbContext _context;
        public UserRepository(PatunganDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
           => await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<UserModel?> GetByEmailAsync(string email)
           => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<UserModel?> GetByIdAsync(int id)
           => await _context.Users.FindAsync(id);

        public async Task AddUserAsync(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}

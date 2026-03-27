using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Patungan.DataAccess.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly PatunganDbContext _context;
        public TransactionTypeRepository(PatunganDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionTypeModel?> GetByIdAsync(int id)
        {
            return await _context.TransactionTypes.FindAsync(id);
        }

        public async Task<IReadOnlyList<TransactionTypeModel>> GetTransactionTypesByUserIdAsync(int userId)
        {
            return await _context.TransactionTypes
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TransactionTypeModel> transactionTypes)
        {
            await _context.TransactionTypes.AddRangeAsync(transactionTypes);
            await _context.SaveChangesAsync();
        }
    }
}

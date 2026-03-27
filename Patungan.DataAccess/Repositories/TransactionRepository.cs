using Microsoft.EntityFrameworkCore;
using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PatunganDbContext _context;

        public TransactionRepository(PatunganDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionModel?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.TransactionType)
                .Include(t => t.MonthlySummary)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TransactionModel>> GetByMonthlySummaryIdAsync(int monthlySummaryId)
        {
            return await _context.Transactions
                .Include(t => t.TransactionType)
                .Where(t => t.MonthlySummaryId == monthlySummaryId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<List<TransactionModel>> GetByUserAndDateRangeAsync(int userId, DateOnly from, DateOnly to)
        {
            return await _context.Transactions
                .Include(t => t.TransactionType)
                .Include(t => t.MonthlySummary)
                .Where(t => t.UserId == userId && t.Date >= from && t.Date <= to)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<List<TransactionModel>> GetByUserAndTypeAsync(int userId, int transactionTypeId, DateOnly from, DateOnly to)
        {
            return await _context.Transactions
                .Include(t => t.TransactionType)
                .Include(t => t.MonthlySummary)
                .Where(t => t.UserId == userId 
                    && t.TransactionTypeId == transactionTypeId 
                    && t.Date >= from 
                    && t.Date <= to)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalByMonthlySummaryAndNatureAsync(int monthlySummaryId, TransactionNature nature)
        {
            return await _context.Transactions
                .Include(t => t.TransactionType)
                .Where(t => t.MonthlySummaryId == monthlySummaryId && t.TransactionType.Nature == nature)
                .SumAsync(t => t.Amount);
        }

        public async Task AddAsync(TransactionModel transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TransactionModel transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TransactionModel transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Transactions.AnyAsync(t => t.Id == id);
        }
    }
}

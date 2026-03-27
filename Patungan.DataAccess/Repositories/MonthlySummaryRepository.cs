using Microsoft.EntityFrameworkCore;
using Patungan.DataAccess.Contexts;
using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;

namespace Patungan.DataAccess.Repositories
{
    public class MonthlySummaryRepository : IMonthlySummaryRepository
    {
        private readonly PatunganDbContext _context;

        public MonthlySummaryRepository(PatunganDbContext context)
        {
            _context = context;
        }

        public async Task<MonthlySummaryModel?> GetByIdAsync(int id)
        {
            return await _context.MonthlySummaries
                .Include(ms => ms.Transactions)
                    .ThenInclude(t => t.TransactionType)
                .FirstOrDefaultAsync(ms => ms.Id == id);
        }

        public async Task<MonthlySummaryModel?> GetByUserAndMonthAsync(int userId, int year, int month)
        {
            return await _context.MonthlySummaries
                .Include(ms => ms.Transactions)
                    .ThenInclude(t => t.TransactionType)
                .FirstOrDefaultAsync(ms => ms.UserId == userId && ms.Year == year && ms.Month == month);
        }

        public async Task<List<MonthlySummaryModel>> GetByUserAndYearAsync(int userId, int year)
        {
            return await _context.MonthlySummaries
                .Include(ms => ms.Transactions)
                    .ThenInclude(t => t.TransactionType)
                .Where(ms => ms.UserId == userId && ms.Year == year)
                .OrderBy(ms => ms.Month)
                .ToListAsync();
        }

        public async Task<List<MonthlySummaryModel>> GetByUserAndDateRangeAsync(int userId, int fromYear, int fromMonth, int toYear, int toMonth)
        {
            return await _context.MonthlySummaries
                .Include(ms => ms.Transactions)
                    .ThenInclude(t => t.TransactionType)
                .Where(ms => ms.UserId == userId 
                    && ((ms.Year == fromYear && ms.Month >= fromMonth) || ms.Year > fromYear)
                    && ((ms.Year == toYear && ms.Month <= toMonth) || ms.Year < toYear))
                .OrderBy(ms => ms.Year)
                .ThenBy(ms => ms.Month)
                .ToListAsync();
        }

        public async Task<MonthlySummaryModel?> GetPreviousMonthAsync(int userId, int year, int month)
        {
            var previousYear = month == 1 ? year - 1 : year;
            var previousMonth = month == 1 ? 12 : month - 1;

            return await _context.MonthlySummaries
                .FirstOrDefaultAsync(ms => ms.UserId == userId 
                    && ms.Year == previousYear 
                    && ms.Month == previousMonth);
        }

        public async Task AddAsync(MonthlySummaryModel summary)
        {
            _context.MonthlySummaries.Add(summary);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MonthlySummaryModel summary)
        {
            _context.MonthlySummaries.Update(summary);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId, int year, int month)
        {
            return await _context.MonthlySummaries
                .AnyAsync(ms => ms.UserId == userId && ms.Year == year && ms.Month == month);
        }
    }
}

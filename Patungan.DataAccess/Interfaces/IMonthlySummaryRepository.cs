using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Interfaces
{
    public interface IMonthlySummaryRepository
    {
        Task<MonthlySummaryModel?> GetByIdAsync(int id);
        Task<MonthlySummaryModel?> GetByUserAndMonthAsync(int userId, int year, int month);
        Task<List<MonthlySummaryModel>> GetByUserAndYearAsync(int userId, int year);
        Task<List<MonthlySummaryModel>> GetByUserAndDateRangeAsync(int userId, int fromYear, int fromMonth, int toYear, int toMonth);
        Task<MonthlySummaryModel?> GetPreviousMonthAsync(int userId, int year, int month);
        Task AddAsync(MonthlySummaryModel summary);
        Task UpdateAsync(MonthlySummaryModel summary);
        Task<bool> ExistsAsync(int userId, int year, int month);
    }
}

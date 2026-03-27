using Patungan.Shared.Responses.MonthlySummary;

namespace Patungan.Services.Interfaces
{
    public interface IMonthlySummaryService
    {
        Task<MonthlySummaryResponse> GetOrCreateMonthlySummaryAsync(int userId, int year, int month);
        Task<MonthlySummaryResponse> GetMonthlySummaryAsync(int userId, int year, int month);
        Task<List<MonthlySummaryResponse>> GetYearlySummariesAsync(int userId, int year);
        Task RecalculateMonthlySummaryAsync(int userId, int year, int month);
        Task CarryOverBudgetToNextMonthAsync(int userId, int year, int month);
    }
}

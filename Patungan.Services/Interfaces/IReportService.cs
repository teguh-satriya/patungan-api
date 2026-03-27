
using Patungan.Shared.Responses.Report;

namespace Patungan.Services.Interfaces
{
    public interface IReportService
    {
        Task<CashFlowReportResponse> GetCashFlowReportAsync(int userId, int year, int month);
        Task<IncomeExpenseComparisonResponse> GetIncomeExpenseComparisonAsync(int userId, int startYear, int startMonth, int endYear, int endMonth);
        Task<TrendAnalysisResponse> GetTrendAnalysisAsync(int userId, int months = 6);
    }
}

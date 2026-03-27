using Patungan.Shared.Responses.Budget;

namespace Patungan.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetOverviewResponse> GetBudgetOverviewAsync(int userId, int year, int month);
        Task<List<TransactionTypeBudgetResponse>> GetSpendingByTypeAsync(int userId, int year, int month);
        Task<decimal> GetProjectedEndingBalanceAsync(int userId, int year, int month);
        Task<CarryoverHistoryResponse> GetCarryoverHistoryAsync(int userId, int fromYear, int fromMonth, int toYear, int toMonth);
    }
}

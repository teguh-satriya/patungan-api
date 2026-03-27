using Patungan.DataAccess.Entities;
using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionModel?> GetByIdAsync(int id);
        Task<List<TransactionModel>> GetByMonthlySummaryIdAsync(int monthlySummaryId);
        Task<List<TransactionModel>> GetByUserAndDateRangeAsync(int userId, DateOnly from, DateOnly to);
        Task<List<TransactionModel>> GetByUserAndTypeAsync(int userId, int transactionTypeId, DateOnly from, DateOnly to);
        Task<decimal> GetTotalByMonthlySummaryAndNatureAsync(int monthlySummaryId, TransactionNature nature);
        Task AddAsync(TransactionModel transaction);
        Task UpdateAsync(TransactionModel transaction);
        Task DeleteAsync(TransactionModel transaction);
        Task<bool> ExistsAsync(int id);
    }
}

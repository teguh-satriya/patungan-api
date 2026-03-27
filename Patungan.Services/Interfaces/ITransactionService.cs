using Patungan.Shared.Requests.Transaction;
using Patungan.Shared.Responses.Transaction;

namespace Patungan.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request);
        Task<TransactionResponse> UpdateTransactionAsync(int transactionId, UpdateTransactionRequest request);
        Task DeleteTransactionAsync(int transactionId);
        Task<List<TransactionResponse>> GetMonthlyTransactionsAsync(int userId, int year, int month);
        Task<List<TransactionResponse>> GetTransactionsByTypeAsync(int userId, int transactionTypeId, DateOnly from, DateOnly to);
        Task<decimal> GetMonthlyIncomeAsync(int userId, int year, int month);
        Task<decimal> GetMonthlyExpenseAsync(int userId, int year, int month);
    }
}

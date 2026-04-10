using Patungan.Shared.Requests.TransactionType;
using Patungan.Shared.Responses.TransactionType;

namespace Patungan.Services.Interfaces
{
    public interface ITransactionTypeService
    {
        Task<TransactionTypeResponse> AddTransactionTypeAsync(CreateTransactionTypeRequest request);
        Task<IReadOnlyList<TransactionTypeResponse>> GetTransactionTypesByUserAsync(int userId);
        Task<TransactionTypeResponse> UpdateTransactionTypeAsync(UpdateTransactionTypeRequest request);
        Task DeleteTransactionTypeAsync(int id);
    }
}

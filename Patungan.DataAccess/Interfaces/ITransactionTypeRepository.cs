using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Interfaces
{
    public interface ITransactionTypeRepository
    {
        Task<TransactionTypeModel?> GetByIdAsync(int id);
        Task<IReadOnlyList<TransactionTypeModel>> GetTransactionTypesByUserIdAsync(int userId);
        Task AddAsync(TransactionTypeModel transactionType);
        Task UpdateAsync(TransactionTypeModel transactionType);
        Task DeleteAsync(TransactionTypeModel transactionType);
        Task AddRangeAsync(IEnumerable<TransactionTypeModel> transactionTypes);
    }
}

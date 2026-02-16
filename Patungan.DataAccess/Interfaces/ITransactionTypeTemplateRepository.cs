using Patungan.DataAccess.Entities;

namespace Patungan.DataAccess.Interfaces
{
    public interface ITransactionTypeTemplateRepository
    {
        Task<IReadOnlyList<TransactionTypeTemplateModel>> GetDefaultTemplatesAsync();
    }
}

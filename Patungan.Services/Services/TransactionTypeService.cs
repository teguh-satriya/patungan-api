using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Shared.Requests.TransactionType;
using Patungan.Shared.Responses.TransactionType;

namespace Patungan.Services.Services
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;

        public TransactionTypeService(ITransactionTypeRepository transactionTypeRepository)
        {
            _transactionTypeRepository = transactionTypeRepository;
        }

        public async Task<TransactionTypeResponse> AddTransactionTypeAsync(CreateTransactionTypeRequest request)
        {
            var entity = new TransactionTypeModel
            {
                Name = request.Name,
                Nature = request.Nature,
                Description = request.Description,
                Icon = request.Icon,
                UserId = request.UserId
            };

            await _transactionTypeRepository.AddAsync(entity);

            return new TransactionTypeResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Icon = entity.Icon,
                Nature = entity.Nature.ToString(),
                Description = entity.Description
            };
        }

        public async Task<IReadOnlyList<TransactionTypeResponse>> GetTransactionTypesByUserAsync(int userId)
        {
            var types = await _transactionTypeRepository.GetTransactionTypesByUserIdAsync(userId);
            return types.Select(t => new TransactionTypeResponse
            {
                Id = t.Id,
                Name = t.Name,
                Icon = t.Icon,
                Nature = t.Nature.ToString(),
                Description = t.Description
            }).ToList();
        }

        public async Task<TransactionTypeResponse> UpdateTransactionTypeAsync(UpdateTransactionTypeRequest request)
        {
            var entity = await _transactionTypeRepository.GetByIdAsync(request.Id);
            if (entity == null) throw new Exception("Transaction type not found");

            entity.Name = request.Name;
            entity.Icon = request.Icon;
            entity.Nature = request.Nature;
            entity.Description = request.Description;

            await _transactionTypeRepository.UpdateAsync(entity);

            return new TransactionTypeResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Icon = entity.Icon,
                Nature = entity.Nature.ToString(),
                Description = entity.Description,
                UserId = entity.UserId
            };
        }

        public async Task DeleteTransactionTypeAsync(int id)
        {
            var entity = await _transactionTypeRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Transaction type not found");

            await _transactionTypeRepository.DeleteAsync(entity);
        }
    }
}

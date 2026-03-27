using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Shared.Constants;
using Patungan.Shared.Requests.Transaction;
using Patungan.Shared.Responses.Transaction;

namespace Patungan.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMonthlySummaryRepository _monthlySummaryRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly IMonthlySummaryService _monthlySummaryService;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IMonthlySummaryRepository monthlySummaryRepository,
            ITransactionTypeRepository transactionTypeRepository,
            IMonthlySummaryService monthlySummaryService)
        {
            _transactionRepository = transactionRepository;
            _monthlySummaryRepository = monthlySummaryRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _monthlySummaryService = monthlySummaryService;
        }

        public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var transactionType = await _transactionTypeRepository.GetByIdAsync(request.TransactionTypeId);
            if (transactionType == null)
                throw new Exception("Transaction type not found");

            var monthlySummary = await _monthlySummaryService.GetOrCreateMonthlySummaryAsync(
                request.UserId, 
                request.Date.Year, 
                request.Date.Month);

            var transaction = new TransactionModel
            {
                MonthlySummaryId = monthlySummary.Id,
                TransactionTypeId = request.TransactionTypeId,
                Date = request.Date,
                Amount = request.Amount,
                Notes = request.Notes,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);

            await _monthlySummaryService.RecalculateMonthlySummaryAsync(
                request.UserId, 
                request.Date.Year, 
                request.Date.Month);

            var createdTransaction = await _transactionRepository.GetByIdAsync(transaction.Id);
            return MapToResponse(createdTransaction!);
        }

        public async Task<TransactionResponse> UpdateTransactionAsync(int transactionId, UpdateTransactionRequest request)
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
                throw new Exception("Transaction not found");

            var oldMonthlySummary = transaction.MonthlySummary;
            var transactionType = await _transactionTypeRepository.GetByIdAsync(request.TransactionTypeId);
            if (transactionType == null)
                throw new Exception("Transaction type not found");

            var needsNewSummary = transaction.Date.Year != request.Date.Year || 
                                  transaction.Date.Month != request.Date.Month;

            if (needsNewSummary)
            {
                var newSummary = await _monthlySummaryService.GetOrCreateMonthlySummaryAsync(
                    transaction.UserId, 
                    request.Date.Year, 
                    request.Date.Month);
                transaction.MonthlySummaryId = newSummary.Id;
            }

            transaction.TransactionTypeId = request.TransactionTypeId;
            transaction.Date = request.Date;
            transaction.Amount = request.Amount;
            transaction.Notes = request.Notes;

            await _transactionRepository.UpdateAsync(transaction);

            await _monthlySummaryService.RecalculateMonthlySummaryAsync(
                oldMonthlySummary.UserId, 
                oldMonthlySummary.Year, 
                oldMonthlySummary.Month);

            if (needsNewSummary)
            {
                await _monthlySummaryService.RecalculateMonthlySummaryAsync(
                    transaction.UserId, 
                    request.Date.Year, 
                    request.Date.Month);
            }

            var updatedTransaction = await _transactionRepository.GetByIdAsync(transactionId);
            return MapToResponse(updatedTransaction!);
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
                throw new Exception("Transaction not found");

            var monthlySummary = transaction.MonthlySummary;
            await _transactionRepository.DeleteAsync(transaction);

            await _monthlySummaryService.RecalculateMonthlySummaryAsync(
                monthlySummary.UserId, 
                monthlySummary.Year, 
                monthlySummary.Month);
        }

        public async Task<List<TransactionResponse>> GetMonthlyTransactionsAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            if (summary == null)
                return new List<TransactionResponse>();

            var transactions = await _transactionRepository.GetByMonthlySummaryIdAsync(summary.Id);
            return transactions.Select(MapToResponse).ToList();
        }

        public async Task<List<TransactionResponse>> GetTransactionsByTypeAsync(int userId, int transactionTypeId, DateOnly from, DateOnly to)
        {
            var transactions = await _transactionRepository.GetByUserAndTypeAsync(userId, transactionTypeId, from, to);
            return transactions.Select(MapToResponse).ToList();
        }

        public async Task<decimal> GetMonthlyIncomeAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            if (summary == null)
                return 0;

            return await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Income);
        }

        public async Task<decimal> GetMonthlyExpenseAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            if (summary == null)
                return 0;

            return await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Outcome);
        }

        private TransactionResponse MapToResponse(TransactionModel transaction)
        {
            return new TransactionResponse
            {
                Id = transaction.Id,
                MonthlySummaryId = transaction.MonthlySummaryId,
                TransactionTypeId = transaction.TransactionTypeId,
                TransactionTypeName = transaction.TransactionType.Name,
                TransactionNature = transaction.TransactionType.Nature.ToString(),
                Date = transaction.Date,
                Amount = transaction.Amount,
                Notes = transaction.Notes,
                UserId = transaction.UserId,
                CreatedAt = transaction.CreatedAt
            };
        }
    }
}

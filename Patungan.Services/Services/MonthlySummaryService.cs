using Patungan.DataAccess.Entities;
using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Shared.Constants;
using Patungan.Shared.Responses.MonthlySummary;

namespace Patungan.Services.Services
{
    public class MonthlySummaryService : IMonthlySummaryService
    {
        private readonly IMonthlySummaryRepository _monthlySummaryRepository;
        private readonly ITransactionRepository _transactionRepository;

        public MonthlySummaryService(
            IMonthlySummaryRepository monthlySummaryRepository,
            ITransactionRepository transactionRepository)
        {
            _monthlySummaryRepository = monthlySummaryRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<MonthlySummaryResponse> GetOrCreateMonthlySummaryAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            
            if (summary == null)
            {
                var previousMonth = await _monthlySummaryRepository.GetPreviousMonthAsync(userId, year, month);
                var startingBalance = previousMonth?.CarriedOver ?? 0;

                summary = new MonthlySummaryModel
                {
                    UserId = userId,
                    Year = year,
                    Month = month,
                    StartingBalance = startingBalance,
                    EndingBalance = startingBalance,
                    CarriedOver = 0,
                    CreatedAt = DateTime.UtcNow
                };

                await _monthlySummaryRepository.AddAsync(summary);
            }

            return MapToResponse(summary);
        }

        public async Task<MonthlySummaryResponse> GetMonthlySummaryAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            if (summary == null)
                throw new Exception("Monthly summary not found");

            return MapToResponse(summary);
        }

        public async Task<List<MonthlySummaryResponse>> GetYearlySummariesAsync(int userId, int year)
        {
            var summaries = await _monthlySummaryRepository.GetByUserAndYearAsync(userId, year);
            return summaries.Select(MapToResponse).ToList();
        }

        public async Task RecalculateMonthlySummaryAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            if (summary == null)
                return;

            var totalIncome = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Income);

            var totalExpense = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Outcome);

            summary.EndingBalance = summary.StartingBalance + totalIncome - totalExpense;
            summary.CarriedOver = summary.EndingBalance;

            await _monthlySummaryRepository.UpdateAsync(summary);
        }

        public async Task CarryOverBudgetToNextMonthAsync(int userId, int year, int month)
        {
            var currentSummary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            if (currentSummary == null)
                throw new Exception("Current month summary not found");

            await RecalculateMonthlySummaryAsync(userId, year, month);

            var nextYear = month == 12 ? year + 1 : year;
            var nextMonth = month == 12 ? 1 : month + 1;

            var nextSummary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, nextYear, nextMonth);
            
            if (nextSummary != null)
            {
                nextSummary.StartingBalance = currentSummary.CarriedOver;
                await _monthlySummaryRepository.UpdateAsync(nextSummary);
                await RecalculateMonthlySummaryAsync(userId, nextYear, nextMonth);
            }
        }

        private MonthlySummaryResponse MapToResponse(MonthlySummaryModel summary)
        {
            var totalIncome = summary.Transactions
                .Where(t => t.TransactionType.Nature == TransactionNature.Income)
                .Sum(t => t.Amount);

            var totalExpense = summary.Transactions
                .Where(t => t.TransactionType.Nature == TransactionNature.Outcome)
                .Sum(t => t.Amount);

            return new MonthlySummaryResponse
            {
                Id = summary.Id,
                UserId = summary.UserId,
                Year = summary.Year,
                Month = summary.Month,
                StartingBalance = summary.StartingBalance,
                EndingBalance = summary.EndingBalance,
                CarriedOver = summary.CarriedOver,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                CreatedAt = summary.CreatedAt
            };
        }
    }
}

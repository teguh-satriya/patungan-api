using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Shared.Constants;
using Patungan.Shared.Responses.Budget;

namespace Patungan.Services.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IMonthlySummaryRepository _monthlySummaryRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BudgetService(
            IMonthlySummaryRepository monthlySummaryRepository,
            ITransactionRepository transactionRepository)
        {
            _monthlySummaryRepository = monthlySummaryRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<BudgetOverviewResponse> GetBudgetOverviewAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            
            if (summary == null)
            {
                return new BudgetOverviewResponse
                {
                    Year = year,
                    Month = month,
                    StartingBalance = 0,
                    TotalIncome = 0,
                    TotalExpense = 0,
                    CurrentBalance = 0,
                    CarriedOverFromPrevious = 0,
                    ProjectedEndingBalance = 0,
                    TransactionCount = 0
                };
            }

            var totalIncome = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Income);

            var totalExpense = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Outcome);

            var currentBalance = summary.StartingBalance + totalIncome - totalExpense;
            var previousMonth = await _monthlySummaryRepository.GetPreviousMonthAsync(userId, year, month);

            return new BudgetOverviewResponse
            {
                Year = year,
                Month = month,
                StartingBalance = summary.StartingBalance,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                CurrentBalance = currentBalance,
                CarriedOverFromPrevious = previousMonth?.CarriedOver ?? 0,
                ProjectedEndingBalance = currentBalance,
                TransactionCount = summary.Transactions.Count
            };
        }

        public async Task<List<TransactionTypeBudgetResponse>> GetSpendingByTypeAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            
            if (summary == null || !summary.Transactions.Any())
                return new List<TransactionTypeBudgetResponse>();

            var totalAmount = summary.Transactions.Sum(t => t.Amount);

            var groupedTransactions = summary.Transactions
                .GroupBy(t => new { t.TransactionTypeId, t.TransactionType.Name, t.TransactionType.Nature })
                .Select(g => new TransactionTypeBudgetResponse
                {
                    TransactionTypeId = g.Key.TransactionTypeId,
                    TransactionTypeName = g.Key.Name,
                    Nature = g.Key.Nature.ToString(),
                    TotalAmount = g.Sum(t => t.Amount),
                    TransactionCount = g.Count(),
                    Percentage = totalAmount > 0 ? (g.Sum(t => t.Amount) / totalAmount) * 100 : 0
                })
                .OrderByDescending(t => t.TotalAmount)
                .ToList();

            return groupedTransactions;
        }

        public async Task<decimal> GetProjectedEndingBalanceAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            
            if (summary == null)
                return 0;

            var totalIncome = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Income);

            var totalExpense = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Outcome);

            return summary.StartingBalance + totalIncome - totalExpense;
        }

        public async Task<CarryoverHistoryResponse> GetCarryoverHistoryAsync(int userId, int fromYear, int fromMonth, int toYear, int toMonth)
        {
            var summaries = await _monthlySummaryRepository.GetByUserAndDateRangeAsync(
                userId, 
                fromYear, 
                fromMonth, 
                toYear, 
                toMonth);

            var history = summaries.Select(s => new MonthlyCarryoverItem
            {
                Year = s.Year,
                Month = s.Month,
                CarriedOver = s.CarriedOver,
                StartingBalance = s.StartingBalance,
                EndingBalance = s.EndingBalance
            }).ToList();

            var totalCarriedOver = summaries.Sum(s => s.CarriedOver);
            var averageCarryover = summaries.Any() ? totalCarriedOver / summaries.Count : 0;

            return new CarryoverHistoryResponse
            {
                History = history,
                TotalCarriedOver = totalCarriedOver,
                AverageCarryover = averageCarryover
            };
        }
    }
}

using Patungan.DataAccess.Interfaces;
using Patungan.Services.Interfaces;
using Patungan.Shared.Constants;
using Patungan.Shared.Responses.Report;

namespace Patungan.Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IMonthlySummaryRepository _monthlySummaryRepository;
        private readonly ITransactionRepository _transactionRepository;

        public ReportService(
            IMonthlySummaryRepository monthlySummaryRepository,
            ITransactionRepository transactionRepository)
        {
            _monthlySummaryRepository = monthlySummaryRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<CashFlowReportResponse> GetCashFlowReportAsync(int userId, int year, int month)
        {
            var summary = await _monthlySummaryRepository.GetByUserAndMonthAsync(userId, year, month);
            
            if (summary == null)
            {
                return new CashFlowReportResponse
                {
                    Year = year,
                    Month = month,
                    OpeningBalance = 0,
                    TotalIncome = 0,
                    TotalExpense = 0,
                    NetCashFlow = 0,
                    ClosingBalance = 0,
                    IncomeDetails = new List<CashFlowDetailItem>(),
                    ExpenseDetails = new List<CashFlowDetailItem>()
                };
            }

            var totalIncome = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Income);

            var totalExpense = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                summary.Id, 
                TransactionNature.Outcome);

            var incomeTransactions = summary.Transactions
                .Where(t => t.TransactionType.Nature == TransactionNature.Income)
                .GroupBy(t => t.TransactionType.Name)
                .Select(g => new CashFlowDetailItem
                {
                    TransactionTypeName = g.Key,
                    Amount = g.Sum(t => t.Amount),
                    TransactionCount = g.Count()
                })
                .ToList();

            var expenseTransactions = summary.Transactions
                .Where(t => t.TransactionType.Nature == TransactionNature.Outcome)
                .GroupBy(t => t.TransactionType.Name)
                .Select(g => new CashFlowDetailItem
                {
                    TransactionTypeName = g.Key,
                    Amount = g.Sum(t => t.Amount),
                    TransactionCount = g.Count()
                })
                .ToList();

            return new CashFlowReportResponse
            {
                Year = year,
                Month = month,
                OpeningBalance = summary.StartingBalance,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                NetCashFlow = totalIncome - totalExpense,
                ClosingBalance = summary.EndingBalance,
                IncomeDetails = incomeTransactions,
                ExpenseDetails = expenseTransactions
            };
        }

        public async Task<IncomeExpenseComparisonResponse> GetIncomeExpenseComparisonAsync(
            int userId, 
            int startYear, 
            int startMonth, 
            int endYear, 
            int endMonth)
        {
            var summaries = await _monthlySummaryRepository.GetByUserAndDateRangeAsync(
                userId, 
                startYear, 
                startMonth, 
                endYear, 
                endMonth);

            var monthlyBreakdown = new List<MonthlyComparisonItem>();
            decimal totalIncome = 0;
            decimal totalExpense = 0;

            foreach (var summary in summaries)
            {
                var income = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                    summary.Id, 
                    TransactionNature.Income);

                var expense = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                    summary.Id, 
                    TransactionNature.Outcome);

                totalIncome += income;
                totalExpense += expense;

                monthlyBreakdown.Add(new MonthlyComparisonItem
                {
                    Year = summary.Year,
                    Month = summary.Month,
                    Income = income,
                    Expense = expense,
                    NetAmount = income - expense,
                    CarriedOver = summary.CarriedOver
                });
            }

            return new IncomeExpenseComparisonResponse
            {
                StartDate = new DateOnly(startYear, startMonth, 1),
                EndDate = new DateOnly(endYear, endMonth, DateTime.DaysInMonth(endYear, endMonth)),
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                NetAmount = totalIncome - totalExpense,
                MonthlyBreakdown = monthlyBreakdown
            };
        }

        public async Task<TrendAnalysisResponse> GetTrendAnalysisAsync(int userId, int months = 6)
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddMonths(-months + 1);

            var summaries = await _monthlySummaryRepository.GetByUserAndDateRangeAsync(
                userId,
                startDate.Year,
                startDate.Month,
                endDate.Year,
                endDate.Month);

            if (!summaries.Any())
            {
                return new TrendAnalysisResponse
                {
                    MonthsAnalyzed = 0,
                    AverageIncome = 0,
                    AverageExpense = 0,
                    AverageNetAmount = 0,
                    HighestIncome = 0,
                    HighestExpense = 0,
                    LowestIncome = 0,
                    LowestExpense = 0,
                    MonthlyTrends = new List<MonthlyTrendItem>(),
                    TrendDirection = "Neutral"
                };
            }

            var monthlyTrends = new List<MonthlyTrendItem>();
            var incomes = new List<decimal>();
            var expenses = new List<decimal>();

            foreach (var summary in summaries)
            {
                var income = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                    summary.Id,
                    TransactionNature.Income);

                var expense = await _transactionRepository.GetTotalByMonthlySummaryAndNatureAsync(
                    summary.Id,
                    TransactionNature.Outcome);

                incomes.Add(income);
                expenses.Add(expense);

                monthlyTrends.Add(new MonthlyTrendItem
                {
                    Year = summary.Year,
                    Month = summary.Month,
                    Income = income,
                    Expense = expense,
                    NetAmount = income - expense,
                    Balance = summary.EndingBalance
                });
            }

            var avgIncome = incomes.Any() ? incomes.Average() : 0;
            var avgExpense = expenses.Any() ? expenses.Average() : 0;
            var trendDirection = DetermineTrendDirection(monthlyTrends);

            return new TrendAnalysisResponse
            {
                MonthsAnalyzed = summaries.Count,
                AverageIncome = avgIncome,
                AverageExpense = avgExpense,
                AverageNetAmount = avgIncome - avgExpense,
                HighestIncome = incomes.Any() ? incomes.Max() : 0,
                HighestExpense = expenses.Any() ? expenses.Max() : 0,
                LowestIncome = incomes.Any() ? incomes.Min() : 0,
                LowestExpense = expenses.Any() ? expenses.Min() : 0,
                MonthlyTrends = monthlyTrends,
                TrendDirection = trendDirection
            };
        }

        private string DetermineTrendDirection(List<MonthlyTrendItem> trends)
        {
            if (trends.Count < 2)
                return "Neutral";

            var firstHalf = trends.Take(trends.Count / 2).Average(t => t.Balance);
            var secondHalf = trends.Skip(trends.Count / 2).Average(t => t.Balance);

            if (secondHalf > firstHalf * 1.1m)
                return "Improving";
            else if (secondHalf < firstHalf * 0.9m)
                return "Declining";
            else
                return "Stable";
        }
    }
}

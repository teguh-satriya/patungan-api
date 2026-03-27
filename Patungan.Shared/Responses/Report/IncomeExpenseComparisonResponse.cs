namespace Patungan.Shared.Responses.Report
{
    public class IncomeExpenseComparisonResponse
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetAmount { get; set; }
        public List<MonthlyComparisonItem> MonthlyBreakdown { get; set; } = new();
    }

    public class MonthlyComparisonItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal NetAmount { get; set; }
        public decimal CarriedOver { get; set; }
    }
}

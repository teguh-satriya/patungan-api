namespace Patungan.Shared.Responses.Report
{
    public class TrendAnalysisResponse
    {
        public int MonthsAnalyzed { get; set; }
        public decimal AverageIncome { get; set; }
        public decimal AverageExpense { get; set; }
        public decimal AverageNetAmount { get; set; }
        public decimal HighestIncome { get; set; }
        public decimal HighestExpense { get; set; }
        public decimal LowestIncome { get; set; }
        public decimal LowestExpense { get; set; }
        public List<MonthlyTrendItem> MonthlyTrends { get; set; } = new();
        public string TrendDirection { get; set; } = string.Empty;
    }

    public class MonthlyTrendItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Balance { get; set; }
    }
}

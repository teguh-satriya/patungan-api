namespace Patungan.Shared.Responses.Report
{
    public class CashFlowReportResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetCashFlow { get; set; }
        public decimal ClosingBalance { get; set; }
        public List<CashFlowDetailItem> IncomeDetails { get; set; } = new();
        public List<CashFlowDetailItem> ExpenseDetails { get; set; } = new();
    }

    public class CashFlowDetailItem
    {
        public string TransactionTypeName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int TransactionCount { get; set; }
    }
}

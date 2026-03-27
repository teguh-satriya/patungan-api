namespace Patungan.Shared.Responses.Budget
{
    public class BudgetOverviewResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CarriedOverFromPrevious { get; set; }
        public decimal ProjectedEndingBalance { get; set; }
        public int TransactionCount { get; set; }
    }
}

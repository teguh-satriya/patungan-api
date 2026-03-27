namespace Patungan.Shared.Responses.Budget
{
    public class TransactionTypeBudgetResponse
    {
        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; } = string.Empty;
        public string Nature { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
        public decimal Percentage { get; set; }
    }
}

namespace Patungan.Shared.Responses.Transaction
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public int MonthlySummaryId { get; set; }
        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; } = string.Empty;
        public string TransactionTypeIcon { get; set; } = string.Empty;
        public string TransactionNature { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

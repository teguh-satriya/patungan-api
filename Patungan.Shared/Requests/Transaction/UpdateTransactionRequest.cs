namespace Patungan.Shared.Requests.Transaction
{
    public class UpdateTransactionRequest
    {
        public int TransactionTypeId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}

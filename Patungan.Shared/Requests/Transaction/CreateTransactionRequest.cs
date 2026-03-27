namespace Patungan.Shared.Requests.Transaction
{
    public class CreateTransactionRequest
    {
        public int UserId { get; set; }
        public int TransactionTypeId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}

namespace Patungan.DataAccess.Entities
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public int MonthlySummaryId { get; set; }
        public MonthlySummaryModel MonthlySummary { get; set; } = default!;
        public int TransactionTypeId { get; set; }
        public TransactionTypeModel TransactionType { get; set; } = default!;
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int UserId { get; set; }
        public UserModel User { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
namespace Patungan.DataAccess.Entities
{
    public class MonthlySummaryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; } = default!;
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public decimal CarriedOver { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}
namespace Patungan.DataAccess.Entities
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;  
        public string PasswordHash { get; set; } = string.Empty;
        public string GoogleId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<MonthlySummaryModel> MonthlySummaries { get; set; } = new List<MonthlySummaryModel>();
        public ICollection<TransactionTypeModel> TransactionTypes { get; set; } = new List<TransactionTypeModel>();
        public ICollection<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}

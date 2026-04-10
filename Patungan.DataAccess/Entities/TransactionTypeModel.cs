using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Entities
{
    public class TransactionTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nature { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "Category";
        public int UserId { get; set; }
        public UserModel User { get; set; } = default!;
        public ICollection<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}
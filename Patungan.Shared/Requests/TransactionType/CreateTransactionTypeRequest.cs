using Patungan.Shared.Constants;

namespace Patungan.Shared.Requests.TransactionType
{
    public class CreateTransactionTypeRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionNature Nature { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "Category";
    }
}

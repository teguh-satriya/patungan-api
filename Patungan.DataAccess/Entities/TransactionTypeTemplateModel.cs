using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Entities
{
    public class TransactionTypeTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionNature Nature { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

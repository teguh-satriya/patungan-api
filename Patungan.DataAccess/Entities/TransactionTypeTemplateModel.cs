using Patungan.Shared.Constants;

namespace Patungan.DataAccess.Entities
{
    public class TransactionTypeTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nature { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "Category";
    }
}

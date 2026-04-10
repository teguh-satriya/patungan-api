using Patungan.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Patungan.Shared.Requests.TransactionType
{
    public class UpdateTransactionTypeRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(income|outcome)$", ErrorMessage = "Nature must be either 'income' or 'outcome'")]
        [Description("Transaction nature. Allowed values: 'income' or 'outcome'")]
        public string Nature { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "Category";
    }
}

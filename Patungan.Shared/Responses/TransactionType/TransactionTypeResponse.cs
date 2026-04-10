namespace Patungan.Shared.Responses.TransactionType
{
    public class TransactionTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Nature { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}

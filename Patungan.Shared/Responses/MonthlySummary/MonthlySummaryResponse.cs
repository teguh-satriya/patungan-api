namespace Patungan.Shared.Responses.MonthlySummary
{
    public class MonthlySummaryResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public decimal CarriedOver { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

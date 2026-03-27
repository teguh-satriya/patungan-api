namespace Patungan.Shared.Responses.Budget
{
    public class CarryoverHistoryResponse
    {
        public List<MonthlyCarryoverItem> History { get; set; } = new();
        public decimal TotalCarriedOver { get; set; }
        public decimal AverageCarryover { get; set; }
    }

    public class MonthlyCarryoverItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal CarriedOver { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal EndingBalance { get; set; }
    }
}

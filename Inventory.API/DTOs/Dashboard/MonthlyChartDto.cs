namespace Inventory.API.DTOs.Dashboard
{
    public class MonthlyChartDto
    {
        public string Month { get; set; } = string.Empty;

        public decimal Purchase { get; set; }

        public decimal Sale { get; set; }
    }
}
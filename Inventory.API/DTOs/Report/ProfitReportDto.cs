namespace Inventory.API.DTOs.Report
{
    public class ProfitReportDto
    {
        public int SaleId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public decimal SalesAmount { get; set; }

        public decimal PurchaseCost { get; set; }

        public decimal GrossProfit { get; set; }

        public decimal ProfitPercentage { get; set; }
    }
}
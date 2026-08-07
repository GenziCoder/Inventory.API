namespace Inventory.API.DTOs.Report
{
    public class SalesReportDto
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Remarks { get; set; }
    }
}
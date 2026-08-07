namespace Inventory.API.DTOs.Report
{
    public class PurchaseReportDto
    {
        public int Id { get; set; }

        public string PurchaseNumber { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Remarks { get; set; }
    }
}
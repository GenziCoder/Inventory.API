namespace Inventory.API.DTOs.Report
{
    public class SupplierPurchaseReportDto
    {
        public int SupplierId { get; set; }

        public string SupplierCode { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int TotalPurchases { get; set; }

        public decimal TotalPurchaseAmount { get; set; }

        public decimal AveragePurchaseAmount { get; set; }

        public DateTime? LastPurchaseDate { get; set; }
    }
}
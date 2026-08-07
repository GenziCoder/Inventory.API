namespace Inventory.API.DTOs.Report
{
    public class StockReportDto
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int StockQuantity { get; set; }

        public int MinimumStock { get; set; }

        public decimal StockValue { get; set; }

        public bool IsLowStock { get; set; }

        public bool IsOutOfStock { get; set; }
    }
}
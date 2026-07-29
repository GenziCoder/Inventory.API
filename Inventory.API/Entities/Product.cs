using Inventory.API.Common;

namespace Inventory.API.Entities
{
    public class Product : BaseEntity
    {
        public string ProductCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int StockQuantity { get; set; }

        public int MinimumStock { get; set; }

        public string? Barcode { get; set; }

        public bool IsActive { get; set; } = true;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();


    }
}
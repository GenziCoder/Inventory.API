using Inventory.API.Common;

namespace Inventory.API.Entities
{
    public class Purchase : BaseEntity
    {
        public string PurchaseNumber { get; set; } = string.Empty;

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Remarks { get; set; }

        public ICollection<PurchaseDetail>? PurchaseDetails { get; set; }= new List<PurchaseDetail>();
            
    }
}
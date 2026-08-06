using Inventory.API.Common;

namespace Inventory.API.Entities
{
    public class Sale : BaseEntity
    {
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;
        public string? Remarks { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    }
}
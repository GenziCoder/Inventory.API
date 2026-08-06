namespace Inventory.API.DTOs.Purchase
{
    public class PurchaseDto
    {
        public int Id { get; set; }

        public string PurchaseNumber { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;
        public int SupplierId { get; set; }


        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Remarks { get; set; }

        public List<PurchaseItemDto> Items { get; set; } = new();
    }
}
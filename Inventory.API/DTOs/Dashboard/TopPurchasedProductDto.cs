namespace Inventory.API.DTOs.Dashboard
{
    public class TopPurchasedProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int QuantityPurchased { get; set; }

        public decimal PurchaseAmount { get; set; }
    }
}
namespace Inventory.API.DTOs.Dashboard
{
    public class TopSellingProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int QuantitySold { get; set; }

        public decimal SalesAmount { get; set; }
    }
}
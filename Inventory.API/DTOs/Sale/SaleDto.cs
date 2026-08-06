namespace Inventory.API.DTOs.Sale
{
    public class SaleDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string SaleNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Remarks { get; set; }

        public List<SaleItemDto> Items { get; set; } = [];

    }
}
using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Sale
{
    public class SaleItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Sale
{
    public class CreateSaleDto
    {
        public int CustomerId { get; set; }
        //public string? CustomerName { get; set; }

        //public string? CustomerPhone { get; set; }

        public DateTime SaleDate { get; set; }

        public string? Remarks { get; set; }

        [MinLength(1)]
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Sale
{
    public class UpdateSaleDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Required]
        [MinLength(1)]
        public List<UpdateSaleItemDto> Items { get; set; } = [];
    }

    public class UpdateSaleItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal UnitPrice { get; set; }
    }


}

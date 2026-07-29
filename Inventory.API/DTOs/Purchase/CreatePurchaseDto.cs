using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Purchase
{
    public class CreatePurchaseDto
    {
        [Required]
        public int SupplierId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string? Remarks { get; set; }

        [MinLength(1)]
        public List<PurchaseItemDto> Items { get; set; } = new();
    }
}
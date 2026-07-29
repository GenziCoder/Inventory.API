using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SellingPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int MinimumStock { get; set; }

        public string? Barcode { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
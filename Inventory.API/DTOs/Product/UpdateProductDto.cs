using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Product
{
    public class UpdateProductDto
    {
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

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
    }
}
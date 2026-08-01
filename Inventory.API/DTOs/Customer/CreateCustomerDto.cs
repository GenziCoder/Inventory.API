using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Customer
{
    public class CreateCustomerDto
    {
        [Required]
        [MaxLength(20)]
        public string CustomerCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string? State { get; set; } = string.Empty;

        public string? Country { get; set; } = string.Empty;

        public string? PostalCode { get; set; } = string.Empty;
    }
}
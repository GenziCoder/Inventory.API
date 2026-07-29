namespace Inventory.API.DTOs.Customer
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string CustomerCode { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
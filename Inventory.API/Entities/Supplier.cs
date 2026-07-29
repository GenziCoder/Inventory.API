using Inventory.API.Common;

namespace Inventory.API.Entities
{
    public class Supplier : BaseEntity
    {
        public string SupplierCode { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    
    }
}
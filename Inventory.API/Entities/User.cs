using Inventory.API.Common;

namespace Inventory.API.Entities
{
    //public class User : BaseEntity
    //{
    //    public string FirstName { get; set; } = string.Empty;

    //    public string LastName { get; set; } = string.Empty;

    //    public string Email { get; set; } = string.Empty;

    //    public string PasswordHash { get; set; } = string.Empty;

    //    public string Role { get; set; } = "Employee";

    //    public bool IsActive { get; set; } = true;



    //    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    //}

    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }=UserRole.Employee;

        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginDate { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

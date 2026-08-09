using Inventory.API.Common;

namespace Inventory.API.DTOs.Auth
{
    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Employee;
    }
}
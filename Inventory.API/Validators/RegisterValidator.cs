using FluentValidation;
using Inventory.API.DTOs.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.MiddleName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(30);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^[6-9]\d{9}$")
            .WithMessage("Please enter a valid 10-digit mobile number.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d")
            .WithMessage("Password must contain at least one number.")
            .Matches(@"[!@#$%^&*(),.?""':{}|<>]")
            .WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");

        RuleFor(x => x.Role)
            .NotEmpty();
    }
}
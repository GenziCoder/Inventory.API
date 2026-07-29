using FluentValidation;
using Inventory.API.DTOs.Category;

namespace Inventory.API.Validators
{
    public class CreateCategoryValidator
        : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")

                .MinimumLength(3)
                .WithMessage("Category name must be at least 3 characters.")

                .MaximumLength(100)
                .WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
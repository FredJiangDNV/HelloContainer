using FluentValidation;
using HelloContainer.Application.DTOs;

namespace HelloContainer.Application.Validators
{
    public class CreateContainerDtoValidator : AbstractValidator<CreateContainerDto>
    {
        public CreateContainerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Container name is required.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage("Capacity must be greater than zero.")
                .LessThanOrEqualTo(10000)
                .WithMessage("Capacity cannot exceed 10,000 units.");
        }
    }
} 
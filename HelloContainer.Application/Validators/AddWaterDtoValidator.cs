using FluentValidation;
using HelloContainer.DTOs;

namespace HelloContainer.Application.Validators
{
    public class AddWaterDtoValidator : AbstractValidator<AddWaterDto>
    {
        public AddWaterDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.")
                .LessThanOrEqualTo(10000)
                .WithMessage("Amount cannot exceed 10,000 units.");
        }
    }
} 
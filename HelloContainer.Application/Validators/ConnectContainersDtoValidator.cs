using FluentValidation;
using HelloContainer.DTOs;

namespace HelloContainer.Application.Validators
{
    public class ConnectContainersDtoValidator : AbstractValidator<ConnectContainersDto>
    {
        public ConnectContainersDtoValidator()
        {
            RuleFor(x => x.SourceContainerId)
                .NotEmpty()
                .WithMessage("Source container ID is required.");

            RuleFor(x => x.TargetContainerId)
                .NotEmpty()
                .WithMessage("Target container ID is required.");

            RuleFor(x => x.SourceContainerId)
                .NotEqual(x => x.TargetContainerId)
                .WithMessage("Source and target containers cannot be the same.");
        }
    }
} 
using MediatR;

namespace HelloContainer.SharedKernel
{
    public record DomainEvent(Guid id) : INotification;
}

using MediatR;

namespace HelloContainer.Domain.Primitives
{
    public record DomainEvent(Guid id) : INotification;
}

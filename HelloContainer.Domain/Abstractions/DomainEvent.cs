using MediatR;

namespace HelloContainer.Domain.Common
{
    public record DomainEvent(Guid id) : INotification;
}

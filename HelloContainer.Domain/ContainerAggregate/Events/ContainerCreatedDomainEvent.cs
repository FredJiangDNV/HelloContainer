using HelloContainer.Domain.Common;

namespace HelloContainer.Domain.ContainerAggregate.Events
{
    public record ContainerCreatedDomainEvent(Guid id, Guid containerId, string name) : DomainEvent(id);
}

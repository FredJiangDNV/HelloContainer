using HelloContainer.Domain.Primitives;

namespace HelloContainer.Domain.Events
{
    public record ContainerCreatedDomainEvent(Guid id, Guid containerId) : DomainEvent(id);
}

using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.ContainerAggregate.Events
{
    public record ContainerDeletedDomainEvent(Guid id, Guid containerId, string name) : DomainEvent(id);
}

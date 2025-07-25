using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.ContainerAggregate.Events
{
    public record WaterOverflowedDomainEvent(Guid id, Guid containerId, string name) : DomainEvent(id);
} 
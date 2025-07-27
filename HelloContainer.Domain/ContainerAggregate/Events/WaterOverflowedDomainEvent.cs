using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.ContainerAggregate.Events;

public record WaterOverflowedDomainEvent(Guid ContainerId, string Name) : DomainEvent; 
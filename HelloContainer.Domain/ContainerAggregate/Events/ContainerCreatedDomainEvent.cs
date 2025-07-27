using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.ContainerAggregate.Events;

public record ContainerCreatedDomainEvent(Guid ContainerId, string Name) : DomainEvent;

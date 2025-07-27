using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.ContainerAggregate.Events;

public record ContainerDeletedDomainEvent(Guid ContainerId, string Name) : DomainEvent;

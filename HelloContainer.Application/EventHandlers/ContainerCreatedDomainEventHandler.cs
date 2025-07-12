using HelloContainer.Common.IntegrationEvents;
using HelloContainer.Domain.ContainerAggregate.Events;
using MassTransit;
using MediatR;

namespace HelloContainer.Application.EventHandlers
{
    public class ContainerCreatedDomainEventHandler(IBus bus) : INotificationHandler<ContainerCreatedDomainEvent>
    {
        public async Task Handle(ContainerCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            var ie = new ContainerCreatedIntegrationEvent(@event.id, @event.containerId, @event.name);
            await bus.Publish(ie, cancellationToken);
        }
    }
}

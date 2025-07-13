using HelloContainer.Common.IntegrationEvents;
using HelloContainer.Domain.ContainerAggregate.Events;
using MassTransit;
using MediatR;

namespace HelloContainer.Application.EventHandlers
{
    public class ContainerDeletedDomainEventHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<ContainerDeletedDomainEvent>
    {
        public async Task Handle(ContainerDeletedDomainEvent @event, CancellationToken cancellationToken)
        {
            var ie = new ContainerDeletedIntegrationEvent(@event.id, @event.containerId, @event.name);
            await publishEndpoint.Publish(ie, cancellationToken);
        }
    }
}

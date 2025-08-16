using HelloContainer.Domain.ContainerAggregate.Events;
using HelloContainer.SharedKernel.IntegrationEvents;
using MassTransit;
using MediatR;

namespace HelloContainer.Application.EventHandlers
{
    public class ContainerDeletedDomainEventHandler : INotificationHandler<ContainerDeletedDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ContainerDeletedDomainEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(ContainerDeletedDomainEvent @event, CancellationToken cancellationToken)
        {
            var ie = new ContainerDeletedIntegrationEvent(@event.Id, @event.ContainerId, @event.Name);
            await _publishEndpoint.Publish(ie, cancellationToken);
        }
    }
}

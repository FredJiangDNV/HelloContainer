using HelloContainer.Domain.ContainerAggregate.Events;
using HelloContainer.SharedKernel.IntegrationEvents;
using MassTransit;
using MediatR;

namespace HelloContainer.Application.EventHandlers
{
    public class ContainerCreatedDomainEventHandler : INotificationHandler<ContainerCreatedDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ContainerCreatedDomainEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(ContainerCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            var ie = new ContainerCreatedIntegrationEvent(@event.Id, @event.ContainerId, @event.Name);
            await _publishEndpoint.Publish(ie, cancellationToken);
        }
    }
}

using HelloContainer.Common;
using HelloContainer.Common.IntegrationEvents;
using HelloContainer.Domain.ContainerAggregate.Events;
using HelloContainer.Domain.OutboxAggregate;
using HelloContainer.Infrastructure;
using MediatR;
using System.Text.Json;

namespace HelloContainer.Application.EventHandlers
{
    public class OutboxWriterEventHandler
        : INotificationHandler<ContainerCreatedDomainEvent>,
          INotificationHandler<ContainerDeletedDomainEvent>
    {
        private readonly HelloContainerDbContext _dbContext;

        public OutboxWriterEventHandler(HelloContainerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(ContainerCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            var ie = new ContainerCreatedIntegrationEvent(@event.id, @event.containerId, @event.name);
            await AddOutboxIntegrationEventAsync(ie);
        }

        public async Task Handle(ContainerDeletedDomainEvent @event, CancellationToken cancellationToken)
        {
            var ie = new ContainerDeletedIntegrationEvent(@event.id, @event.containerId, @event.name);
            await AddOutboxIntegrationEventAsync(ie);
        }

        private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent)
        {
            var content = JsonSerializer.Serialize(integrationEvent);

            await _dbContext.OutboxIntegrationEvents.AddAsync(OutboxIntegrationEvent.Create(
                integrationEvent.GetType().Name,
                content));

            await _dbContext.SaveChangesAsync();
        }
    }
}

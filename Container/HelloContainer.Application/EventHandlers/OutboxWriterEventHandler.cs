using HelloContainer.Domain.ContainerAggregate.Events;
using HelloContainer.Domain.OutboxAggregate;
using HelloContainer.Infrastructure;
using HelloContainer.SharedKernel;
using HelloContainer.SharedKernel.IntegrationEvents;
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
            var ie = new ContainerCreatedIntegrationEvent(@event.Id, @event.ContainerId, @event.Name);
            var content = JsonSerializer.Serialize(ie);
            await AddOutboxIntegrationEventAsync(ie, content);
        }

        public async Task Handle(ContainerDeletedDomainEvent @event, CancellationToken cancellationToken)
        {
            var ie = new ContainerDeletedIntegrationEvent(@event.Id, @event.ContainerId, @event.Name);
            var content = JsonSerializer.Serialize(ie);
            await AddOutboxIntegrationEventAsync(ie, content);
        }

        private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent, string content)
        {
            await _dbContext.OutboxIntegrationEvents.AddAsync(OutboxIntegrationEvent.Create(
                integrationEvent.GetType().Name,
                content));

            await _dbContext.SaveChangesAsync();
        }
    }
}

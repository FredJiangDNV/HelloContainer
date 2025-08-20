using HelloContainer.Domain.ContainerAggregate.Events;
using HelloContainer.Domain.OutboxAggregate;
using HelloContainer.Domain.Abstractions;
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
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OutboxWriterEventHandler(
            IOutboxRepository outboxRepository,
            IUnitOfWork unitOfWork)
        {
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ContainerCreatedDomainEvent @event, CancellationToken cancellationToken)
        {
            var integrationEvent = new ContainerCreatedIntegrationEvent(@event.Id, @event.ContainerId, @event.Name);
            await AddOutboxIntegrationEventAsync(integrationEvent, cancellationToken);
        }

        public async Task Handle(ContainerDeletedDomainEvent @event, CancellationToken cancellationToken)
        {
            var integrationEvent = new ContainerDeletedIntegrationEvent(@event.Id, @event.ContainerId, @event.Name);
            await AddOutboxIntegrationEventAsync(integrationEvent, cancellationToken);
        }

        private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            var content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());
            var outboxEvent = OutboxIntegrationEvent.Create(
                integrationEvent.GetType().Name,
                content);

            _outboxRepository.Add(outboxEvent);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

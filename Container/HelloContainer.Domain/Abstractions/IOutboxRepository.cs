using HelloContainer.Domain.OutboxAggregate;

namespace HelloContainer.Domain.Abstractions
{
    public interface IOutboxRepository : IRepository<OutboxIntegrationEvent>
    {
        Task<IEnumerable<OutboxIntegrationEvent>> GetUnprocessedEventsAsync(CancellationToken cancellationToken = default);
        Task MarkAsProcessedAsync(Guid eventId, CancellationToken cancellationToken = default);
    }
}

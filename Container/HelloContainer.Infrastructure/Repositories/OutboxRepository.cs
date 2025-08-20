using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.OutboxAggregate;
using Microsoft.EntityFrameworkCore;

namespace HelloContainer.Infrastructure.Repositories
{
    public class OutboxRepository : Repository<OutboxIntegrationEvent>, IOutboxRepository
    {
        public OutboxRepository(HelloContainerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OutboxIntegrationEvent>> GetUnprocessedEventsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<OutboxIntegrationEvent>()
                .Where(e => !e.Processed)
                .ToListAsync(cancellationToken);
        }

        public async Task MarkAsProcessedAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            var outboxEvent = await _context.Set<OutboxIntegrationEvent>()
                .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
            
            if (outboxEvent != null)
            {
                outboxEvent.MarkAsProcessed();
                _context.Update(outboxEvent);
            }
        }
    }
}

using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.AlertAggregate;
using Microsoft.EntityFrameworkCore;

namespace HelloContainer.Infrastructure.Repositories
{
    public class AlertRepository : Repository<Alert>, IAlertRepository
    {
        public AlertRepository(HelloContainerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Alert>> FindByContainerIdAsync(Guid containerId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Alert>()
                .Where(a => a.ContainerId == containerId)
                .ToListAsync(cancellationToken);
        }
    }
} 
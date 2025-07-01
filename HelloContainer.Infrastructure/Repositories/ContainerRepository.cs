using HelloContainer.Domain;
using HelloContainer.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HelloContainer.Infrastructure.Repositories
{
    public class ContainerRepository : Repository<Container>, IContainerRepository
    {
        public ContainerRepository(HelloContainerDbContext context) : base(context)
        {
        }

        public virtual async Task<IEnumerable<Container>> FindAsync(Expression<Func<Container, bool>> predict, CancellationToken cancellationToken = default)
        => await _context.Set<Container>().Where(predict).ToListAsync(cancellationToken);
    }
}

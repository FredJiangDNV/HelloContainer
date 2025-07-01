using HelloContainer.Domain;
using HelloContainer.Infrastructure.Common;
using System.Linq.Expressions;

namespace HelloContainer.Infrastructure.Repositories
{
    public interface IContainerRepository : IRepository<Container>
    {
        Task<IEnumerable<Container>> FindAsync(Expression<Func<Container, bool>> predict, CancellationToken cancellationToken = default);
    }
}

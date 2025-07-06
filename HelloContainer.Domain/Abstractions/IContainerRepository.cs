using HelloContainer.Domain.ContainerAggregate;
using System.Linq.Expressions;

namespace HelloContainer.Domain.Abstractions
{
    public interface IContainerRepository : IRepository<Container>
    {
        Task<IEnumerable<Container>> FindAsync(Expression<Func<Container, bool>> predict, CancellationToken cancellationToken = default);
    }
}

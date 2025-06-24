namespace HelloContainer.Infrastructure.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        HelloContainerDbContext DbContext { get; }
    }
}
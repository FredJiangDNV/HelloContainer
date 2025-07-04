namespace HelloContainer.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        public HelloContainerDbContext DbContext { get; }

        public UnitOfWork(HelloContainerDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
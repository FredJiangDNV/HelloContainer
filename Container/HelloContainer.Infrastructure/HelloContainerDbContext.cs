using HelloContainer.Infrastructure.EntityConfigs;
using HelloContainer.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HelloContainer.Infrastructure
{
    public class HelloContainerDbContext : DbContext
    {
        private readonly IPublisher _publisher;

        public HelloContainerDbContext(DbContextOptions<HelloContainerDbContext> options, IPublisher publisher) : base(options)
        {
            _publisher = publisher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContainerEntityConfig());
            modelBuilder.ApplyConfiguration(new AlertEntityConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            
            await PublishDomainEventsAsync();

            return result;
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<AggregateRoot>()
                .Select(entry => entry.Entity.PopDomainEvents())
                .SelectMany(x => x)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }
    }
}

using HelloContainer.Domain.OutboxAggregate;
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

        public DbSet<OutboxIntegrationEvent> OutboxIntegrationEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContainerEntityConfig());
            modelBuilder.ApplyConfiguration(new OutboxIntegrationEventConfig());
            modelBuilder.ApplyConfiguration(new AlertEntityConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await PublishDomainEventsAsync();

            var result = await base.SaveChangesAsync(cancellationToken);

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

using HelloContainer.Domain.Common;
using HelloContainer.Domain.OutboxAggregate;
using HelloContainer.Infrastructure.EntityConfigs;
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
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .Where(e => e.DomainEvents.Any())
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();

                    entity.ClearDomainEvents();

                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }
    }
}

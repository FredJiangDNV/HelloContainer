using HelloContainer.Domain.Primitives;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContainerEntityConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Before

            var result = await base.SaveChangesAsync(cancellationToken);

            // After
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

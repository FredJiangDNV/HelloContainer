using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HelloContainer.Domain.OutboxAggregate;

namespace HelloContainer.Infrastructure.EntityConfigs
{
    public class OutboxIntegrationEventConfig : IEntityTypeConfiguration<OutboxIntegrationEvent>
    {
        public void Configure(EntityTypeBuilder<OutboxIntegrationEvent> builder)
        {
            builder.ToContainer("outboxs")
               .HasPartitionKey(c => c.Id)
               .HasDiscriminator<string>("Discriminator");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ToJsonProperty("id");
            builder.Property(o => o.EventName).ToJsonProperty("eventName");
            builder.Property(o => o.EventContent).ToJsonProperty("eventContent");

            builder.Ignore(t => t.DomainEvents);
        }
    }
}

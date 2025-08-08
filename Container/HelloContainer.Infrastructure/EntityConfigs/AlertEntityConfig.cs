using HelloContainer.Domain.AlertAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloContainer.Infrastructure.EntityConfigs
{
    public class AlertEntityConfig : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.ToContainer("alerts")
               .HasPartitionKey(c => c.Id)
               .HasDiscriminator<string>("Discriminator");

            builder.HasKey(a => a.Id);
            builder.Property(c => c.Id).ToJsonProperty("id");

            builder.Property(a => a.ContainerId).IsRequired();
            builder.Property(a => a.Message).IsRequired().HasMaxLength(256);
            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Ignore(t => t.DomainEvents);
        }
    }
} 
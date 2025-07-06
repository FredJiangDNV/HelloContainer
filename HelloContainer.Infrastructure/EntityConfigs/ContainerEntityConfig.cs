using HelloContainer.Domain.ContainerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloContainer.Infrastructure.EntityConfigs
{
    public class ContainerEntityConfig : IEntityTypeConfiguration<Container>
    {
        public void Configure(EntityTypeBuilder<Container> builder)
        {
            builder.ToContainer("containers")
                .HasPartitionKey(c => c.Id)
                .HasDiscriminator<string>("Discriminator");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ToJsonProperty("id");
            builder.Property(c => c.Name)
                .IsRequired()
                .ToJsonProperty("name");
            
            builder.Property(c => c.Amount)
                .HasConversion(
                    amount => amount.Value,           // Convert to database
                    value => Amount.Create(value))    // Convert from database
                .IsRequired()
                .ToJsonProperty("amount");
            
            builder.Property(c => c.Capacity)
                .HasConversion(
                    capacity => capacity.Value,       // Convert to database
                    value => Capacity.Create(value))  // Convert from database
                .IsRequired()
                .ToJsonProperty("capacity");

            builder.Property(c => c.ConnectedContainerIdsRaw)
             .ToJsonProperty("connectedContainerIds")
             .IsRequired();

            builder.Ignore(t => t.DomainEvents);
        }
    }
}

using HelloContainer.Domain;
using HelloContainer.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloContainer.Infrastructure.EntityConfigs
{
    public class ContainerEntityConfig : IEntityTypeConfiguration<Container>
    {
        public void Configure(EntityTypeBuilder<Container> builder)
        {
            builder.ToContainer("containers")
                .HasDiscriminator<string>("Discriminator");

            builder.HasKey(c => c.Id);
            
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
        }
    }
}

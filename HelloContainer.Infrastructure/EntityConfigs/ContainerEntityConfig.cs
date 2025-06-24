using HelloContainer.Domain;
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
                .IsRequired()
                .ToJsonProperty("amount");
            
            builder.Property(c => c.Capacity)
                .IsRequired()
                .ToJsonProperty("capacity");
        }
    }
}

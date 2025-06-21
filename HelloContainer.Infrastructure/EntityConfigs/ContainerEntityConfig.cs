using HelloContainer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelloContainer.Infrastructure.EntityConfigs
{
    public class ContainerEntityConfig : IEntityTypeConfiguration<Container>
    {
        public void Configure(EntityTypeBuilder<Container> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.ToContainer("containers")
                .HasDiscriminator<string>("Discriminator");
            
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            
            builder.Property(c => c.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            
            builder.Property(c => c.Capacity)
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}

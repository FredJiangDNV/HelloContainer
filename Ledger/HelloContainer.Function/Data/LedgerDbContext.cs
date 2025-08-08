using Microsoft.EntityFrameworkCore;
using HelloContainer.Function.Data.Entities;

namespace HelloContainer.Function.Data
{
    public class LedgerDbContext : DbContext
    {
        public LedgerDbContext(DbContextOptions<LedgerDbContext> options) : base(options)
        {
        }

        public DbSet<EventLedger> EventLedgers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventLedger>(entity =>
            {
                entity.ToContainer("ledgers")
                 .HasPartitionKey(c => c.Id)
                 .HasDiscriminator<string>("Discriminator");

                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id).ToJsonProperty("id");
                entity.Property(e => e.EventType).ToJsonProperty("eventType");
                entity.Property(e => e.EventData).ToJsonProperty("eventData");
                entity.Property(e => e.CreatedAt).ToJsonProperty("createdAt");
            });
        }
    }
} 
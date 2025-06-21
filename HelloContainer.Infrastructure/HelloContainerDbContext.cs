using HelloContainer.Infrastructure.EntityConfigs;
using Microsoft.EntityFrameworkCore;

namespace HelloContainer.Infrastructure
{
    public class HelloContainerDbContext : DbContext
    {
        public HelloContainerDbContext(DbContextOptions<HelloContainerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContainerEntityConfig());
        }
    }
}

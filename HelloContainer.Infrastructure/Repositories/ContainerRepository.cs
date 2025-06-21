using HelloContainer.Domain;
using Microsoft.EntityFrameworkCore;

namespace HelloContainer.Infrastructure.Repositories
{
    public class ContainerRepository : IContainerRepository
    {
        private readonly HelloContainerDbContext _context;

        public ContainerRepository(HelloContainerDbContext context)
        {
            _context = context;
        }

        public async Task<Container?> GetById(Guid id)
        {
            return await _context.Set<Container>()
                .FirstOrDefaultAsync(wc => wc.Id == id);
        }

        public async Task<IEnumerable<Container>> GetAll()
        {
            return await _context.Set<Container>()
                .ToListAsync();
        }

        public async Task<Container> Add(Container container)
        {
            _context.Add(container);
            await _context.SaveChangesAsync();
            return container;
        }

        public async Task<Container> Update(Container container)
        {
            _context.Update(container);
            await _context.SaveChangesAsync();
            return container;
        }

        public async Task Delete(Guid id)
        {
            var container = await GetById(id);
            if (container != null)
            {
                _context.Remove(container);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Set<Container>().AnyAsync(wc => wc.Id == id);
        }
    }
} 
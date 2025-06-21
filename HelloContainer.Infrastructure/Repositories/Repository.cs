using Microsoft.EntityFrameworkCore;

namespace HelloContainer.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly HelloContainerDbContext _context;

        public Repository(HelloContainerDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Set<T>().AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }
    }
} 
using Microsoft.EntityFrameworkCore;

namespace HelloContainer.Infrastructure.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HelloContainerDbContext _context;

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

        public T Add(T entity)
            => _context.Add(entity).Entity;

        public void Update(T entity)
            => _context.Update(entity);

        public void Delete(T entity)
            => _context.Remove(entity);

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Set<T>().AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }
    }
}
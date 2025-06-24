namespace HelloContainer.Infrastructure.Common
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(Guid id);
        Task<bool> Exists(Guid id);
    }
}
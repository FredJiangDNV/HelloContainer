namespace HelloContainer.Domain.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Exists(Guid id);
    }
}
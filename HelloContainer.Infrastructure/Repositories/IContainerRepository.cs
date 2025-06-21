using HelloContainer.Domain;

namespace HelloContainer.Infrastructure.Repositories
{
    public interface IContainerRepository
    {
        public Task<Container?> GetById(Guid id);

        public Task<IEnumerable<Container>> GetAll();

        public Task<Container> Add(Container container);

        public Task<Container> Update(Container container);

        public Task Delete(Guid id);

        public Task<bool> Exists(Guid id);
    }
}

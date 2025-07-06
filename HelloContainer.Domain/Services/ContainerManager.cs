using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.ContainerAggregate;
using HelloContainer.Domain.Exceptions;

namespace HelloContainer.Domain.Services
{
    public class ContainerManager : IContainerManager
    {
        private readonly IContainerRepository _repository;

        public ContainerManager(IContainerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Container?> AddWater(Guid containerId, double amount)
        {
            var container = await _repository.GetById(containerId);
            if (container == null)
            {
                throw new ContainerNotFoundException(containerId);
            }

            container.AddWater(amount);
            return container;
        }

        public async Task<Container> ConnectContainers(Guid sourceContainerId, Guid targetContainerId)
        {
            var sourceContainer = await _repository.GetById(sourceContainerId);
            if (sourceContainer == null)
            {
                throw new ContainerNotFoundException(sourceContainerId);
            }

            var targetContainer = await _repository.GetById(targetContainerId);
            if (targetContainer == null)
            {
                throw new ContainerNotFoundException(targetContainerId);
            }

            sourceContainer.ConnectTo(targetContainer);
            return sourceContainer;
        }
    }
}

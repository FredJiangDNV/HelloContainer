using HelloContainer.Domain.ContainerAggregate;

namespace HelloContainer.Domain.Services
{
    public interface IContainerManager
    {
        Task<Container?> AddWater(Guid containerId, double amount);

        Task<Container> ConnectContainers(Guid sourceContainerId, Guid targetContainerId);

        Task<Container> DisconnectContainers(Guid sourceContainerId, Guid targetContainerId);
    }
}

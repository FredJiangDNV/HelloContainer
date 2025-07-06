using HelloContainer.Application.DTOs;

namespace HelloContainer.Application.Services
{
    public interface IContainerService
    {
        Task<ContainerReadDto> CreateContainer(CreateContainerDto createDto);
        Task<IEnumerable<ContainerReadDto>> GetContainers(string? searchKeyword = null);
        Task<ContainerReadDto?> GetContainerById(Guid id);
        Task<ContainerReadDto> AddWater(Guid containerId, double amount);
        Task<ContainerReadDto> ConnectContainers(Guid sourceContainerId, Guid targetContainerId);
        Task<ContainerReadDto> DisconnectContainers(Guid sourceContainerId, Guid targetContainerId);
    }
} 
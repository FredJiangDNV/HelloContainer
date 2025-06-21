using HelloContainer.Application.DTOs;

namespace HelloContainer.Application.Services
{
    public interface IContainerService
    {
        Task<ContainerReadDto> CreateContainer(CreateContainerDto createDto);
        Task<IEnumerable<ContainerReadDto>> GetContainers();
        Task<ContainerReadDto?> GetContainerById(Guid id);
    }
} 
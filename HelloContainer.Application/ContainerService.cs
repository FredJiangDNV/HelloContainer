using AutoMapper;
using HelloContainer.Application.DTOs;
using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.Services;

namespace HelloContainer.Application
{
    public class ContainerService
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContainerManager _containerManager;
        private readonly IContainerFactory _containerFactory;

        public ContainerService(IContainerRepository containerRepository, IMapper mapper, IUnitOfWork unitOfWork, IContainerManager containerManager, IContainerFactory containerFactory)
        {
            _containerRepository = containerRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _containerManager = containerManager;
            _containerFactory = containerFactory;
        }

        public async Task<ContainerReadDto> CreateContainer(CreateContainerDto createDto)
        {
            var container = await _containerFactory.CreateContainer(createDto.Name, createDto.Capacity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ContainerReadDto>(container);
        }

        public async Task<IEnumerable<ContainerReadDto>> GetContainers(string? searchKeyword = null)
        {
            var containers = string.IsNullOrEmpty(searchKeyword) ?
                await _containerRepository.GetAll() :
                await _containerRepository.FindAsync(x => x.Name.Contains(searchKeyword));
            return _mapper.Map<IEnumerable<ContainerReadDto>>(containers);
        }

        public async Task<ContainerReadDto?> GetContainerById(Guid id)
        {
            var container = await _containerRepository.GetById(id);
            return _mapper.Map<ContainerReadDto>(container);
        }

        public async Task<ContainerReadDto> AddWater(Guid containerId, double amount)
        {
            var container = await _containerManager.AddWater(containerId, amount);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ContainerReadDto>(container);
        }

        public async Task<ContainerReadDto> ConnectContainers(Guid sourceContainerId, Guid targetContainerId)
        {
            var sourceContainer = await _containerManager.ConnectContainers(sourceContainerId, targetContainerId);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ContainerReadDto>(sourceContainer);
        }

        public async Task<ContainerReadDto> DisconnectContainers(Guid sourceContainerId, Guid targetContainerId)
        {
            var sourceContainer = await _containerManager.DisconnectContainers(sourceContainerId, targetContainerId);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ContainerReadDto>(sourceContainer);
        }
    }
}
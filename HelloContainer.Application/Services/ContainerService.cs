using AutoMapper;
using HelloContainer.Application.DTOs;
using HelloContainer.Domain;
using HelloContainer.Infrastructure.Repositories;

namespace HelloContainer.Application.Services
{
    public class ContainerService : IContainerService
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IMapper _mapper;

        public ContainerService(IContainerRepository containerRepository, IMapper mapper)
        {
            _containerRepository = containerRepository;
            _mapper = mapper;
        }

        public async Task<ContainerReadDto> CreateContainer(CreateContainerDto createDto)
        {
            var container = _mapper.Map<Container>(createDto);
            
            var createdContainer = await _containerRepository.Add(container);
            return _mapper.Map<ContainerReadDto>(createdContainer);
        }

        public async Task<IEnumerable<ContainerReadDto>> GetContainers()
        {
            var containers = await _containerRepository.GetAll();
            return _mapper.Map<IEnumerable<ContainerReadDto>>(containers);
        }

        public async Task<ContainerReadDto?> GetContainerById(Guid id)
        {
            var container = await _containerRepository.GetById(id);
            return _mapper.Map<ContainerReadDto>(container);
        }
    }
} 
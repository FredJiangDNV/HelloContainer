using AutoMapper;
using HelloContainer.Application.DTOs;
using HelloContainer.Domain;
using HelloContainer.Infrastructure.Common;
using HelloContainer.Infrastructure.Repositories;

namespace HelloContainer.Application.Services
{
    public class ContainerService : IContainerService
    {
        private readonly IContainerRepository _containerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ContainerService(IContainerRepository containerRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _containerRepository = containerRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ContainerReadDto> CreateContainer(CreateContainerDto createDto)
        {
            var container = Container.Create(createDto.Name, createDto.Capacity);

            _containerRepository.Add(container);

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
    }
} 
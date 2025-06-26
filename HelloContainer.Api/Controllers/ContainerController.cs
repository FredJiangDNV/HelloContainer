using HelloContainer.Application.DTOs;
using HelloContainer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelloContainer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _containerService;

        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        [HttpPost]
        public async Task<ActionResult<ContainerReadDto>> CreateContainer(CreateContainerDto createDto)
        {
            var container = await _containerService.CreateContainer(createDto);
            return CreatedAtAction(nameof(GetContainerById), new { id = container.Id }, container);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerReadDto>>> GetContainers([FromQuery] string? searchKeyword)
        {
            var containers = await _containerService.GetContainers(searchKeyword);
            return Ok(containers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerReadDto>> GetContainerById(Guid id)
        {
            var container = await _containerService.GetContainerById(id);
            return Ok(container);
        }

        [HttpPost("{id}/add-water")]
        public async Task<ActionResult<ContainerReadDto>> AddWater(Guid id, [FromBody] AddWaterDto addWaterDto)
        {
            var container = await _containerService.AddWater(id, addWaterDto.Amount);
            return Ok(container);
        }

        [HttpPost("connect")]
        public async Task<ActionResult<ContainerReadDto>> ConnectContainers([FromBody] ConnectContainersDto connectDto)
        {
            var container = await _containerService.ConnectContainers(connectDto.SourceContainerId, connectDto.TargetContainerId);
            return Ok(container);
        }
    }
}

using HelloContainer.Application;
using HelloContainer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloContainer.Api.Controllers
{
    [Route("api/[controller]s")]
    //[Authorize]
    public class ContainerController : ApiControllerBase
    {
        private readonly ContainerService _containerService;

        public ContainerController(ContainerService containerService)
        {
            _containerService = containerService;
        }

        [HttpPost]
        public async Task<ActionResult<ContainerReadDto>> CreateContainer(CreateContainerDto createDto)
        {
            var result = await _containerService.CreateContainer(createDto);
            if (result.IsFailure)
                return HandleError(result.Error);

            return CreatedAtAction(nameof(GetContainerById), new { id = result.Value.Id }, result.Value);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainer(Guid id)
        {
            await _containerService.DeleteContainer(id);
            return NoContent();
        }

        [HttpPost("{id}/water")]
        public async Task<ActionResult<ContainerReadDto>> AddWater(Guid id, [FromBody] AddWaterDto addWaterDto)
        {
            var container = await _containerService.AddWater(id, addWaterDto.Amount);
            return Ok(container);
        }

        [HttpPost("connections")]
        public async Task<ActionResult<ContainerReadDto>> ConnectContainers([FromBody] ConnectContainersDto connectDto)
        {
            var container = await _containerService.ConnectContainers(connectDto.SourceContainerId, connectDto.TargetContainerId);
            return Ok(container);
        }

        [HttpPost("disconnections")]
        public async Task<ActionResult<ContainerReadDto>> DisConnectContainers([FromBody] DisconnectContainersDto connectDto)
        {
            var container = await _containerService.DisconnectContainers(connectDto.SourceContainerId, connectDto.TargetContainerId);
            return Ok(container);
        }
    }
}

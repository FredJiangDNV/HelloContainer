using HelloContainer.Application.DTOs;
using HelloContainer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelloContainer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;

        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        [HttpPost]
        public async Task<ActionResult<ContainerReadDto>> CreateContainer([FromBody] CreateContainerDto createDto)
        {
            var container = await _containerService.CreateContainer(createDto);
            return CreatedAtAction(nameof(GetContainer), new { id = container.Id }, container);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerReadDto>>> GetContainers()
        {
            var containers = await _containerService.GetContainers();
            return Ok(containers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerReadDto>> GetContainer(Guid id)
        {
            var container = await _containerService.GetContainerById(id);
            if (container == null)
            {
                return NotFound();
            }

            return Ok(container);
        }
    }
}

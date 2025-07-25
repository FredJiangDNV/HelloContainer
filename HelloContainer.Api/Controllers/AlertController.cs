using HelloContainer.Domain.Abstractions;
using HelloContainer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HelloContainer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class AlertController : ControllerBase
    {
        private readonly IAlertRepository _alertRepository;

        public AlertController(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        [HttpGet("{containerId}")]
        public async Task<ActionResult<IEnumerable<AlertReadDto>>> GetAlerts([FromRoute] Guid containerId)
        {
            var alerts = await _alertRepository.FindByContainerIdAsync(containerId);
            var result = alerts.Select(a => new AlertReadDto(a.Id, a.ContainerId, a.Message, a.CreatedAt));
            return Ok(result);
        }
    }
} 
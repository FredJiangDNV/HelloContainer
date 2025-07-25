using HelloContainer.Application;
using HelloContainer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HelloContainer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class AlertController : ControllerBase
    {
        private readonly AlertService _alertService;

        public AlertController(AlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet("{containerId}")]
        public async Task<ActionResult<IEnumerable<AlertReadDto>>> GetAlerts([FromRoute] Guid containerId)
        {
            var alerts = await _alertService.GetAlertsByContainerId(containerId);
            return Ok(alerts);
        }
    }
} 
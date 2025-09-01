using HelloContainer.Application;
using HelloContainer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloContainer.Api.Controllers
{
    [Route("api/[controller]s")]
    //[Authorize]
    public class AlertController : ApiControllerBase
    {
        private readonly AlertService _alertService;

        public AlertController(AlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlertReadDto>>> GetAlerts([FromQuery] Guid containerId)
        {
            var alerts = await _alertService.GetAlertsByContainerId(containerId);
            return Ok(alerts);
        }
    }
} 
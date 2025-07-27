using HelloContainer.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace HelloContainer.Api.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult<T> HandleResult<T>(Result<T> result)
        {
            return result.IsFailure ? HandleError(result.Error) : Ok(result.Value);
        }

        protected ActionResult HandleError(Error error)
        {
            return error.Code switch
            {
                var code when code.StartsWith("NotFound.") => NotFound(error),
                var code when code.StartsWith("Validation.") => BadRequest(error),
                var code when code.StartsWith("Conflict.") => Conflict(error),
                var code when code.StartsWith("Unauthorized.") => Unauthorized(error),
                var code when code.StartsWith("Forbidden.") => Forbid(),
                _ => StatusCode(500, error)
            };
        }
    }
} 
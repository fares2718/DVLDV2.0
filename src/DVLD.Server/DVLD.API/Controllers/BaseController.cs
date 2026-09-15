using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BaseController(ISender sender, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    protected readonly ISender Sender = sender;
    protected readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;

    protected IActionResult HandleErrors(List<Error> errors)
    {
        if (errors.Count == 0)
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError
            );

        var firstError = errors[0];

        return firstError.Type switch
        {
            ErrorType.Validation => BadRequest(errors),

            ErrorType.NotFound => NotFound(errors),

            ErrorType.Conflict => Conflict(errors),

            ErrorType.Unauthorized => Unauthorized(),

            ErrorType.Forbidden => Forbid(),

            _ => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                detail: firstError.Description)
        };
    }
}
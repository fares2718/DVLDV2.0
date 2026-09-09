using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

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
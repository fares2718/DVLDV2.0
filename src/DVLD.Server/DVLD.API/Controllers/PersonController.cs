using DVLD.Application.Features.People.Create;
using DVLD.Contracts.People;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ISender _sender;

        public PersonController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("CreatePerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Create([FromBody]CreatePersonRequest request)
        {
            var cmd = new CreatePersonCommand(
                request.NationalId,
                request.FirstName,
                request.SecondName,
                request.ThirdName,
                request.LastName,
                request.MotherName,
                request.DateOfBirth,
                request.Phone,
                request.Gender,
                request.Email,
                request.NationalityCountryCode,
                request.ImagePath,
                request.AltPhone);
            var result = await _sender.Send(cmd);

            if (result.IsError)
            {
                return result.FirstError.Type switch
                {
                    ErrorType.Validation => BadRequest(result.Errors),
                    _ => Problem(statusCode:StatusCodes.Status500InternalServerError,detail:result.FirstError.Description)
                };
            }

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}

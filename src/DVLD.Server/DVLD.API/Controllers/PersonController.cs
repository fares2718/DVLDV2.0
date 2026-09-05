using DVLD.Application.Features.People.Create;
using DVLD.Application.Features.People.Get;
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

        public async Task<IActionResult> Create([FromBody]CreatePersonCommand cmd,CancellationToken cancellationToken)
        {
            var result = await _sender.Send(cmd, cancellationToken);

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

        [HttpGet("GetPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetPeople([FromQuery]GetPeopleQuery query,CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);
            if(result.IsError)
                return result.FirstError.Type switch
                {
                    ErrorType.Validation => BadRequest(result.Errors),
                    _ => Problem(statusCode:StatusCodes.Status500InternalServerError,detail:result.FirstError.Description)
                };
            return Ok(result.Value);
        }
        
    }
}

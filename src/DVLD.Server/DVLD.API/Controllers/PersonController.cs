using DVLD.Application.Features.People.Activation;
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

        [HttpPatch("activate-person/{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Activate(Guid personId, CancellationToken cancellationToken)
        {
            var cmd = new ActivatePersonCommand(personId);
            var result = await _sender.Send(cmd, cancellationToken);
            
            if(result.IsError)
                return result.FirstError.Type switch
                {
                    ErrorType.Validation => BadRequest(result.Errors),
                    ErrorType.NotFound =>  NotFound(result.Errors),
                    _ => Problem(statusCode:StatusCodes.Status500InternalServerError,detail:result.FirstError.Description)
                };
            return Ok("Person has been activated successfully");
        }

        [HttpPost("create-person")]
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

            return StatusCode(StatusCodes.Status201Created,cmd);
        }

        [HttpPatch("deactivate-person/{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeActivate(Guid personId, CancellationToken cancellationToken)
        {
            var cmd = new DeActivatePersonCommand(personId);
            var result = await _sender.Send(cmd, cancellationToken);
            
            if(result.IsError)
                return result.FirstError.Type switch
                {
                    ErrorType.Validation => BadRequest(result.Errors),
                    ErrorType.NotFound =>  NotFound(result.Errors),
                    _ => Problem(statusCode:StatusCodes.Status500InternalServerError,detail:result.FirstError.Description)
                };
            return Ok("Person has been deactivated successfully");
        }

        [HttpGet("get-person/{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(Guid personId, CancellationToken cancellationToken)
        {
            var query = new GetPersonByIdQuery(personId);
            var result = await _sender.Send(query, cancellationToken);
            if(result.IsError)
                return result.FirstError.Type switch
                {
                    ErrorType.Validation => BadRequest(result.Errors),
                    ErrorType.NotFound =>  NotFound(result.Errors),
                    _ => Problem(statusCode:StatusCodes.Status500InternalServerError,detail:result.FirstError.Description)
                };
            return Ok(result.Value);   
        }

        [HttpGet("filter-people")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAllSummary([FromQuery]GetPeopleQuery query,CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);
            if(result.IsError)
                return result.FirstError.Type switch
                {
                    ErrorType.Validation => BadRequest(result.Errors),
                    ErrorType.NotFound =>  NotFound(result.Errors),
                    _ => Problem(statusCode:StatusCodes.Status500InternalServerError,detail:result.FirstError.Description)
                };
            return Ok(result.Value);
        }
        
    }
}

using DVLD.Application.Features.Addresses.Add;
using DVLD.Application.Features.Addresses.Get;
using DVLD.Contract.Address;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

public class AddressController(ISender sender) : BaseController(sender)
{
    [HttpPost("add-address")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> AddAddress([FromBody] AddAddressCommand cmd,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(cmd, cancellationToken);

        return result.MatchFirst<IActionResult>(
            success => Ok(success),
            error => error.Type switch
            {
                ErrorType.NotFound => NotFound(new
                {
                    code = error.Code,
                    description = error.Description
                }),
                ErrorType.Validation => BadRequest(new
                {
                    code = error.Code,
                    description = error.Description
                }),
                _ => Problem(title: error.Code, detail: error.Description)
            }
        );
    }
    
    [HttpPost("add-addresses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> AddAddresses([FromBody] AddAddressesCommand cmd,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(cmd, cancellationToken);

        return result.MatchFirst<IActionResult>(
            success => Ok(success),
            error => error.Type switch
            {
                ErrorType.NotFound => NotFound(new
                {
                    code = error.Code,
                    description = error.Description
                }),
                ErrorType.Validation => BadRequest(new
                {
                    code = error.Code,
                    description = error.Description
                }),
                _ => Problem(title:error.Code,detail:error.Description)
            }
        );
    }

    [HttpGet("get-person-addresses/{personId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetPersonAddresses(Guid personId,[FromQuery] GetPersonAddressesRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPersonAddressesQuery(
            personId, 
            request.AddressType, 
            request.IsPrimary, 
            request.IsActive
            );

        var result = await _sender.Send(query,cancellationToken);

        return result.MatchFirst(
            success => Ok(success),
            error => error.Type switch
            {
                ErrorType.NotFound => NotFound(new
                {
                    code = error.Code,
                    description = error.Description
                }),
                ErrorType.Validation => BadRequest(new
                {
                    code = error.Code,
                    description = error.Description
                }),
                _ => Problem(title: error.Code, detail: error.Description)
            }
        );

    }
}
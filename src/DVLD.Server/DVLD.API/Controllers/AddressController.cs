using DVLD.Application.Features.Addresses.Add;
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
}
using DVLD.Application.Features.Addresses.Add;
using DVLD.Application.Features.Addresses.Get;
using DVLD.Application.Features.Addresses.Status;
using DVLD.Application.Features.Addresses.Update;
using DVLD.Contract.Address.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

public class AddressController(ISender sender) : BaseController(sender)
{
    [HttpPost("add-address")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> AddAddress([FromBody] AddAddressRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = new AddAddressCommand(request.PersonId,request.AddressType,request.CountryCode,
            request.City,request.Governorate,request.Street,request.BuildingNumber,request.ApartmentNumber,
            request.PostalCode,request.AdditionalDetails);
        
        var result = await Sender.Send(cmd, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);
        return CreatedAtRoute("get-person-addresses",new
        {
            personId = request.PersonId
        },result.Value);
    }
    
    [HttpPost("add-addresses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> AddAddresses([FromBody] AddAddressesRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = new AddAddressesCommand(request.Addresses.Select(
            a => new AddAddressCommand(a.PersonId,a.AddressType,a.CountryCode,
                a.City,a.Governorate,a.Street,a.BuildingNumber,a.ApartmentNumber,
                a.PostalCode,a.AdditionalDetails)
            )
        );
        var result = await Sender.Send(cmd, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);
        
        return CreatedAtRoute("get-person-addresses",new
        {
            personId = request.Addresses.First().PersonId
        },result.Value);
    }

    [HttpGet("get-person-addresses/{personId:guid}",Name = "get-person-addresses")]
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

        var result = await Sender.Send(query,cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);
        
        return Ok(result.Value);

    }

    [HttpPatch("change-address-activation-status/{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeAddressActivationStatus(Guid addressId,
        ChangeAddressActivationStatusRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = new ChangeAddressActivationStatusCommand(addressId, request.IsActive);
        var result = await Sender.Send(cmd, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);
        return Ok("Address activation status has been changed successfully");
    }
    
    [HttpPatch("change-address-primary-status/{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeAddressPrimaryStatus(Guid addressId,
        ChangeAddressPrimaryStatusRequest request, CancellationToken cancellationToken = default)
    {
        var cmd = new ChangeAddressPrimaryStatusCommand(addressId, request.IsPrimary);
        var result = await Sender.Send(cmd, cancellationToken);
        
        if (result.IsError)
            return HandleErrors(result.Errors);
        return Ok("Address primary status has been changed successfully");
    }

    [HttpPut("update-address/{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] UpdateAddressRequest request,
        CancellationToken cancellationToken = default)
    {
        var cmd = new UpdateAddressCommand(addressId, request.AddressType, request.CountryCode
            , request.City, request.Governorate, request.Street, request.BuildingNumber, request.ApartmentNumber,
            request.PostalCode, request.AdditionalDetails);

        var result = await Sender.Send(cmd, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);
        return Ok("Address has been updated successfully");
    }
}
using DVLD.Application.Features.People.Activation;
using DVLD.Application.Features.People.Create;
using DVLD.Application.Features.People.Get;
using DVLD.Application.Features.People.Update;
using DVLD.Contract.Person.Requests;
using DVLD.Contract.Person.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers
{
    public class PersonController(ISender sender) : BaseController(sender)
    {
        [HttpPatch("activate-person/{personId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Activate(Guid personId, CancellationToken cancellationToken = default)
        {
            var cmd = new ActivatePersonCommand(personId);
            var result = await Sender.Send(cmd, cancellationToken);

            if (result.IsError)
                return HandleErrors(result.Errors);
            return Ok("Person has been activated successfully");
        }

        [HttpPost("create-person")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Create([FromBody]CreatePersonRequest request,CancellationToken cancellationToken = default)
        {
            var cmd = new CreatePersonCommand(
                request.NationalId,
                request.FirstName, request.SecondName,request.ThirdName,request.LastName,
                request.MotherName,
                request.DateOfBirth,request.Phone,request.Gender,request.Email,request.NationalityCountryCode,
                request.ImagePath,request.AltPhone
                );
            var result = await Sender.Send(cmd, cancellationToken);

            if (result.IsError)
                return HandleErrors(result.Errors);

            var response = new CreatePersonResponse(result.Value, "Person has been created successfully.");

            return CreatedAtRoute("get-person",new
            {
                personId = result.Value
            },response);
        }

        [HttpPatch("deactivate-person/{personId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeActivate(Guid personId, CancellationToken cancellationToken = default)
        {
            var cmd = new DeActivatePersonCommand(personId);
            var result = await Sender.Send(cmd, cancellationToken);

            if (result.IsError)
                return HandleErrors(result.Errors);
            return Ok("Person has been deactivated successfully");
        }

        [HttpGet("get-person/{personId:guid}", Name = "get-person")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(Guid personId, CancellationToken cancellationToken = default)
        {
            var query = new GetPersonByIdQuery(personId);
            var result = await Sender.Send(query, cancellationToken);
            if (result.IsError)
                return HandleErrors(result.Errors);
            return Ok(result.Value);   
        }

        [HttpGet("filter-people")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAllSummary([FromQuery]GetPeopleRequest request,CancellationToken cancellationToken = default)
        {
            var query = new GetPeopleQuery(
                request.Search,
                request.NationalId,
                request.Name,
                request.Gender,
                request.Phone,
                request.Email,
                request.SortBy,
                request.IsDescending,
                request.IsActive,
                request.PageNumber,
                request.PageSize
                );
            var result = await Sender.Send(query, cancellationToken);
            if (result.IsError)
                return HandleErrors(result.Errors);
            return Ok(result.Value);
        }

        [HttpPatch("update-person-name/{personId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> UpdateName(Guid personId ,
            [FromBody] UpdatePersonNameRequest request,
            CancellationToken cancellationToken = default)
        {
            var cmd = new UpdatePersonNameCommand(
                personId,
                request.FirstName,
                request.SecondName,
                request.ThirdName,
                request.LastName,
                request.MotherName
                );
            var result = await Sender.Send(cmd, cancellationToken);
            if (result.IsError)
                return HandleErrors(result.Errors);
            return Ok("Person name has been updated successfully");
        }
        
        [HttpPatch("update-person-contact-info/{personId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> UpdateContactInfo(Guid personId,
            [FromBody] UpdatePersonContactInfoRequest request,CancellationToken cancellationToken = default)
        {
            var cmd = new UpdatePersonContactInfoCommand(
                personId,
                request.Phone,
                request.AltPhone
                );
            var result = await Sender.Send(cmd, cancellationToken);
            if(result.IsError)
                return HandleErrors(result.Errors);
            return Ok("Person contact info has been updated successfully");
        }
        
        [HttpPatch("update-personal-info/{personId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> UpdatePersonalInfo(Guid personId,
            [FromBody] UpdatePersonalInfoRequest request,CancellationToken cancellationToken = default)
        {
            var cmd = new UpdatePersonalInfoCommand(
                personId,
                request.DateOfBirth,
                request.NationalityCountryCode
                );
            var result = await Sender.Send(cmd, cancellationToken);
            if (result.IsError)
                return HandleErrors(result.Errors);
            return Ok("Personal info has been updated successfully");
        }
        
    }
}

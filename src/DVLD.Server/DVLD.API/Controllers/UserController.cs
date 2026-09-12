using DVLD.Application.Features.User.AccountStatus;
using DVLD.Application.Features.User.Create;
using DVLD.Application.Features.User.Get;
using DVLD.Application.Features.User.Roles;
using DVLD.Application.Filters.User;
using DVLD.Contract.User.Requests;
using DVLD.Contract.User.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

public class UserController(ISender sender) : BaseController(sender)
{

    [HttpPost("create-user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request,CancellationToken cancellationToken = default)
    {
        var cmd = new CreateUserCommand(request.PersonId, request.Username, request.Password, request.RoleId);
        var result = await Sender.Send(cmd, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);
        
        var response = new CreateUserResponse(result.Value,"User has been created");
        
        return CreatedAtRoute("get-user", new { userId = result.Value }, response);
    }

    [HttpGet("get-user/{userId:guid}", Name = "get-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> GetUser(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(userId);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);

        return Ok(result.Value);
    }
    
    [HttpGet("get-user/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> GetUser(string username, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByUsernameQuery(username);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("get-users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> GetUser([FromQuery] GetUsersFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersWithFilterQuery(filter);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsError)
            return HandleErrors(result.Errors);
        
        return Ok(result.Value);
    }

    [HttpPost("assign-user-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> AssignUserRole(AssignUserRoleRequest request,CancellationToken cancellationToken = default)
    {
        var cmd = new AddUserRoleCommand(request.UserId,request.RoleId,request.AssignedByUserId);

        var result = await Sender.Send(cmd,cancellationToken);
        
        if (result.IsError)
            return HandleErrors(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpDelete("remove-user-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> RemoveUserRole(RemoveUserRoleRequest request,CancellationToken cancellationToken = default)
    {
        var cmd = new RemoveUserRoleCommand(request.UserId,request.RoleId);

        var result = await Sender.Send(cmd,cancellationToken);
        
        if (result.IsError)
            return HandleErrors(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpPatch("change-activation-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> ChangeUserActivationStatus(ChangeActivationStatusRequest request,CancellationToken cancellationToken = default)
    {
        var cmd = new ChangeUserActivationStatusCommand(request.UserId,request.IsActive);

        var result = await Sender.Send(cmd,cancellationToken);
        
        if (result.IsError)
            return HandleErrors(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpPatch("change-Lock-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> ChangeUserLockStatus(ChangeLockStatusRequest request,CancellationToken cancellationToken = default)
    {
        var cmd = new ChangeUserLockStatusCommand(request.UserId,request.IsLocked,request.DurationType,request.Duration);

        var result = await Sender.Send(cmd,cancellationToken);
        
        if (result.IsError)
            return HandleErrors(result.Errors);
        
        return Ok(result.Value);
    }
    
}
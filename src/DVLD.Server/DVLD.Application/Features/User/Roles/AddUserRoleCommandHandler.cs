using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Roles;

public sealed class AddUserRoleCommandHandler(IUnitOfWork uow) : IRequestHandler<AddUserRoleCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Success>> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            return Error.Validation("UserID.Invalid","User ID cannot be empty");

        if (request.RoleId < 1)
            return Error.Validation("RoleID.Invalid", "Role ID must be greater than 0");

        try
        {
            var userRole = UserRole.Assign(request.UserId, request.RoleId, request.AssignedBy);
            await _uow.UserRepository.AddRoleAsync(userRole, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation("Domain.Rule.Violation", e.Message);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("NotFound", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}
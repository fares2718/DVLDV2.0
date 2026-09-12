using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Roles;

public sealed class RemoveUserRoleCommandHandler(IUnitOfWork uow)
    : IRequestHandler<RemoveUserRoleCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Success>> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            return Error.Validation("UserID.Invalid","User ID cannot be empty");

        if (request.RoleId < 1)
            return Error.Validation("RoleID.Invalid", "Role ID must be greater than 0");

        try
        {
            await _uow.UserRepository.RemoveRoleAsync(request.UserId,request.RoleId,cancellationToken);
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
using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.AccountStatus;

public class ChangeUserLockStatusCommandHandler(IUnitOfWork uow)
    : IRequestHandler<ChangeUserLockStatusCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Success>> Handle(ChangeUserLockStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            return Error.Validation("UserID.Invalid","User ID cannot be empty");

        try
        {
            await _uow.UserRepository.ChangeUserLockStatusAsync(request.UserId, request.IsLocked,
                cancellationToken:cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return Error.NotFound("User.NotFound", e.Message);
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation("Domain.Rule.Violation", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}
using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.AccountStatus;

public class ChangeUserActivationStatusCommandHandler(IUnitOfWork uow)
    : IRequestHandler<ChangeUserActivationStatusCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Success>> Handle(ChangeUserActivationStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            return Error.Validation("UserID.Invalid","User ID cannot be empty");

        try
        {
            await _uow.UserRepository.ChangeUserActivationStatusAsync(request.UserId, request.IsActive,
                cancellationToken);
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
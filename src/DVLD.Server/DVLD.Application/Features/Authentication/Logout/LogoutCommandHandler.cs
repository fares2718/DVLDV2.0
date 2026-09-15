using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Authentication.Logout;

public sealed class LogoutCommandHandler(IUnitOfWork uow) : IRequestHandler<LogoutCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
            return Error.Validation("User.ID.Invalid","User ID is invalid");

        try
        {
            await _uow.UserRefreshTokenRepository
                .RevokeRefreshTokenAsync(request.UserId, request.RevokedByIp, cancellationToken: cancellationToken);

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
            return Error.NotFound("User.RefreshToken.NotFound", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}
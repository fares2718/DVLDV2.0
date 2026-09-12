using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.AccountStatus;

public sealed record ChangeUserActivationStatusCommand(Guid UserId,bool IsActive) : IRequest<ErrorOr<Success>>;
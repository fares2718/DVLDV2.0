using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.AccountStatus;

public enum LockDurationType
{
    Minutes = 1,
    Hours = 2,
    Days = 3,
    Months = 4,
    Years = 5
};
public sealed record ChangeUserLockStatusCommand(Guid UserId, bool IsLocked
    ,LockDurationType? DurationType = null ,int? LockDuration = null) : IRequest<ErrorOr<Success>>;
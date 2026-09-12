using DVLD.Application.Features.User.AccountStatus;

namespace DVLD.Contract.User.Requests;

public sealed record ChangeLockStatusRequest(Guid UserId,bool IsLocked,LockDurationType? DurationType,int? Duration);
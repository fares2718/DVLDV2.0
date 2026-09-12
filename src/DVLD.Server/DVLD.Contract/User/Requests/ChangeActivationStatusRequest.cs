namespace DVLD.Contract.User.Requests;

public sealed record ChangeActivationStatusRequest(Guid UserId,bool IsActive);
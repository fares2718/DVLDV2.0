namespace DVLD.Contract.User.Requests;

public sealed record RemoveUserRoleRequest(Guid UserId, int RoleId);
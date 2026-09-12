namespace DVLD.Contract.User.Requests;

public sealed record AssignUserRoleRequest(Guid UserId, int RoleId,Guid? AssignedByUserId);
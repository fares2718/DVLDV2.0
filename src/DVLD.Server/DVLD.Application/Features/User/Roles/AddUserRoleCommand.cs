using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Roles;

public sealed record AddUserRoleCommand(Guid UserId,int RoleId,Guid? AssignedBy) : IRequest<ErrorOr<Success>>;
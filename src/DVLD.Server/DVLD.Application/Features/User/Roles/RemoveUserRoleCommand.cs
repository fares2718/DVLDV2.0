using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Roles;

public sealed record RemoveUserRoleCommand(Guid UserId, int RoleId) : IRequest<ErrorOr<Success>>;
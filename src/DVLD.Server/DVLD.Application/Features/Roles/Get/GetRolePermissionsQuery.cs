using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Roles.Get;

public sealed record GetRolePermissionsQuery(List<string> UserRoles) : IRequest<ErrorOr<long>>;
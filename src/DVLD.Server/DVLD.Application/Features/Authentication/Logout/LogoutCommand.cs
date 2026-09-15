using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Authentication.Logout;

public sealed record LogoutCommand(Guid UserId,string? RevokedByIp) : IRequest<ErrorOr<Success>>;
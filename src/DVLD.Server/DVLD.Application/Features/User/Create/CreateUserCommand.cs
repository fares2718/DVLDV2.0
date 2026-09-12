using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.User.Create;

public sealed record CreateUserCommand(
    Guid PersonId,
    string Username,
    string Password,
    int RoleId
    ) : IRequest<ErrorOr<Guid>>;
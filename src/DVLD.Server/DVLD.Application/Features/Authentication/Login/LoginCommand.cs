using DVLD.Application.DTOs;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Authentication.Login;

public sealed record LoginCommand(
    string Username,
    string Password,
    string? CreatedByIp = null,
    string? UserAgent = null
    ) : IRequest<ErrorOr<TokensDto>>;
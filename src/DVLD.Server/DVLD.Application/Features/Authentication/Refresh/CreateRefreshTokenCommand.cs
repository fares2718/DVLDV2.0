using DVLD.Application.DTOs;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Authentication.Refresh;

public sealed record CreateRefreshTokenCommand(
    string RefreshTokenHash,
    string? CreatedByIp,
    string? UserAgent
    ) : IRequest<ErrorOr<TokensDto>>;
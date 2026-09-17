using DVLD.Application.Abstractions.Authentication;
using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Domain.Common;
using MediatR;
using ErrorOr;

namespace DVLD.Application.Features.Authentication.Refresh;

public class CreateRefreshTokenCommandHandler(IUnitOfWork uow, ITokensGenerator tokensGenerator)
    : IRequestHandler<CreateRefreshTokenCommand, ErrorOr<TokensDto>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ITokensGenerator _tokensGenerator = tokensGenerator;
    public async Task<ErrorOr<TokensDto>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tokenRecord =
                await _uow.UserRefreshTokenRepository.GetRefreshTokenRecord(request.RefreshTokenHash,
                    cancellationToken);
            var user = await _uow.UserRepository.GetAsync(tokenRecord.UserId, cancellationToken);
            if (user == null)
                return Error.NotFound("User.NotFound", "User not found");

            var roles = await _uow.UserRepository.GetUserRoles(user.UserId, cancellationToken);
            string accessToken = _tokensGenerator.GenerateToken(user, roles);
            string refreshToken = _tokensGenerator.GenerateRefreshToken();

            string refreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            await _uow.UserRefreshTokenRepository.CreateRefreshTokenAsync(user.UserId, refreshTokenHash,
                DateTime.UtcNow.AddHours(2),
                request.CreatedByIp, request.UserAgent, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return new TokensDto(accessToken, refreshTokenHash);
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation("Domain.Rule.Violation", e.Message);
        }
        catch (KeyNotFoundException)
        {
            return Error.NotFound("User.NotFound", "User not found");
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}
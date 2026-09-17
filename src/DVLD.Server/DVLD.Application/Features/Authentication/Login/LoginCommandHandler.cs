using DVLD.Application.Abstractions.Authentication;
using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Authentication.Login;

public class LoginCommandHandler(IUnitOfWork uow, LoginCommandValidator validator, ITokensGenerator tokensGenerator) : IRequestHandler<LoginCommand, ErrorOr<TokensDto>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly LoginCommandValidator _validator = validator;
    private readonly ITokensGenerator _tokensGenerator = tokensGenerator;

    public async Task<ErrorOr<TokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request,cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation(validationResult.Errors.First().ErrorCode,validationResult.Errors.First().ErrorMessage);

        var user = await _uow.UserRepository.GetAuthModelByUsernameAsync(request.Username,cancellationToken);
        
        if (user == null)
            return Error.Unauthorized("Authentication.Failed","Username or password is incorrect");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            user.RecordFailedLogin();
            await _uow.SaveChangesAsync(cancellationToken);
            return Error.Unauthorized("Authentication.Failed", "Username or password is incorrect");
        }

        try
        {
            var roles = await _uow.UserRepository.GetUserRoles(user.UserId, cancellationToken);
            string accessToken = _tokensGenerator.GenerateToken(user,roles);
            string refreshToken = _tokensGenerator.GenerateRefreshToken();

            string refreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            await _uow.UserRefreshTokenRepository.CreateRefreshTokenAsync(user.UserId,refreshTokenHash,DateTime.UtcNow.AddHours(2),request.CreatedByIp,request.UserAgent, cancellationToken);
            user.RecordSuccessfulLogin();
            await _uow.SaveChangesAsync(cancellationToken);
            return new TokensDto(accessToken, refreshTokenHash);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("Error", e.Message);
        }
    }
}
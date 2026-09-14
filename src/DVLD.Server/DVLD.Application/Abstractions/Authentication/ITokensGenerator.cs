using DVLD.Domain.Views;

namespace DVLD.Application.Abstractions.Authentication;

public interface ITokensGenerator
{
    string GenerateToken(AuthenticationUserView user);
    string GenerateRefreshToken();
}
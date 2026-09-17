using DVLD.Domain.Entities;
using DVLD.Domain.Views;

namespace DVLD.Application.Abstractions.Authentication;

public interface ITokensGenerator
{
    string GenerateToken(User user,List<string> roles);
    string GenerateRefreshToken();
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DVLD.Application.Abstractions.Authentication;
using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Entities;
using DVLD.Domain.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DVLD.Infrastructure.Authentication;

internal sealed class TokensGenerator(IConfiguration configuration) : ITokensGenerator
{
    private readonly IConfiguration _config = configuration; 

    public string GenerateToken(User user,List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Username!)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        double.TryParse(_config["JwtSettings:ExpiryInMinutes"]!,out double expiryInMinutes);
        var tokenValue = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiryInMinutes),
            signingCredentials: creds
        );


        string token = new JwtSecurityTokenHandler().WriteToken(tokenValue);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class UserRefreshTokenRepository(DvldContext dvldContext) : IUserRefreshTokenRepository
{
    private readonly DvldContext _dvldContext = dvldContext;

    public async Task CreateRefreshTokenAsync(Guid userId, string tokenHash, DateTime expiresAt, string? createdByIp = null,
        string? userAgent = null)
    {
        var newToken = UserRefreshToken.Create(userId, tokenHash, expiresAt, createdByIp, userAgent);
        
        await RevokeRefreshTokenAsync(userId,createdByIp, newToken.RefreshTokenId);
        
        _dvldContext.UserRefreshTokens.Add(newToken);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId, string? revokedByIp = null, Guid? replacedByTokenId = null)
    {
        var userRefreshToken = await _dvldContext.UserRefreshTokens.Where(r => r.UserId == userId)
            .OrderBy(r => r.CreatedAt)
            .LastOrDefaultAsync();
        
        if (userRefreshToken is not null)
            userRefreshToken.Revoke(revokedByIp, replacedByTokenId);
    }
}
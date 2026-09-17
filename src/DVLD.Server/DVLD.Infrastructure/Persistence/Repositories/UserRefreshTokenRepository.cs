using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using DVLD.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Infrastructure.Persistence.Repositories;

public class UserRefreshTokenRepository(DvldContext dvldContext) : IUserRefreshTokenRepository
{
    private readonly DvldContext _dvldContext = dvldContext;

    public async Task CreateRefreshTokenAsync(Guid userId, string tokenHash, DateTime expiresAt, string? createdByIp = null,
        string? userAgent = null,CancellationToken cancellationToken = default)
    {
        var newToken = UserRefreshToken.Create(userId, tokenHash, expiresAt, createdByIp, userAgent);
        
        await RevokeRefreshTokenAsync(userId,createdByIp, newToken.RefreshTokenId, cancellationToken);
        
        _dvldContext.UserRefreshTokens.Add(newToken);
    }

    public async Task<UserRefreshToken> GetRefreshTokenRecord(string refreshTokenHash, CancellationToken cancellationToken = default)
    {
        var record = await _dvldContext.UserRefreshTokens.SingleOrDefaultAsync(rt => rt.TokenHash == refreshTokenHash, cancellationToken);
        if (record is null)
            throw new KeyNotFoundException("No refresh token found");
        if(record.IsExpired())
            throw new DomainException("Token is expired");
        if(record.IsRevoked())
            throw new DomainException("Token is revoked");
        return record;
    }

    public async Task RevokeRefreshTokenAsync(Guid userId, string? revokedByIp = null, Guid? replacedByTokenId = null
        ,CancellationToken cancellationToken = default)
    {
        var userRefreshToken = await _dvldContext.UserRefreshTokens.Where(r => r.UserId == userId)
            .OrderBy(r => r.CreatedAt)
            .LastOrDefaultAsync(cancellationToken: cancellationToken);

        if (userRefreshToken is null)
            throw new KeyNotFoundException("User Refresh Token was not found");
        if(!userRefreshToken.IsExpired())
            userRefreshToken.Revoke(revokedByIp, replacedByTokenId);
    }
}
using DVLD.Domain.Entities;

namespace DVLD.Application.Abstractions.Persistence;

public interface IUserRefreshTokenRepository
{
    Task CreateRefreshTokenAsync(Guid userId,
        string tokenHash,
        DateTime expiresAt,
        string? createdByIp = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
    
    Task<UserRefreshToken> GetRefreshTokenRecord(string refreshTokenHash,CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(Guid userId,
        string? revokedByIp = null,
        Guid? replacedByTokenId = null,
        CancellationToken cancellationToken = default);
}
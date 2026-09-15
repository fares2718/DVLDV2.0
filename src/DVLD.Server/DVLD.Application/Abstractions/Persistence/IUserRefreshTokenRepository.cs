namespace DVLD.Application.Abstractions.Persistence;

public interface IUserRefreshTokenRepository
{
    Task CreateRefreshTokenAsync(Guid userId,
        string tokenHash,
        DateTime expiresAt,
        string? createdByIp = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(Guid userId,
        string? revokedByIp = null,
        Guid? replacedByTokenId = null,
        CancellationToken cancellationToken = default);
}
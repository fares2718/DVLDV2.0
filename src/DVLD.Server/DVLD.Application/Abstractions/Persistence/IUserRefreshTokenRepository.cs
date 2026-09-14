namespace DVLD.Application.Abstractions.Persistence;

public interface IUserRefreshTokenRepository
{
    Task CreateRefreshTokenAsync(Guid userId,
        string tokenHash,
        DateTime expiresAt,
        string? createdByIp = null,
        string? userAgent = null);

    Task RevokeRefreshTokenAsync(Guid userId,
        string? revokedByIp = null,
        Guid? replacedByTokenId = null);
}
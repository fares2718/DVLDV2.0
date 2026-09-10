using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class UserRefreshToken
{
    public Guid RefreshTokenId { get; private set; }

    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = null!;

    public DateTime ExpiresAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public Guid? ReplacedByTokenId { get; private set; }

    public string? CreatedByIp { get; private set; }

    public string? RevokedByIp { get; private set; }

    public string? UserAgent { get; private set; }


    // EF Core
    private UserRefreshToken() { }


    private UserRefreshToken(
        Guid userId,
        string tokenHash,
        DateTime expiresAt,
        string? createdByIp,
        string? userAgent)
    {
        RefreshTokenId = Guid.NewGuid();

        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;

        CreatedAt = DateTime.UtcNow;

        CreatedByIp = createdByIp;
        UserAgent = userAgent;
    }


    public static UserRefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTime expiresAt,
        string? createdByIp = null,
        string? userAgent = null)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User ID cannot be empty");

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new DomainException("Token hash cannot be empty");

        if (expiresAt <= DateTime.UtcNow)
            throw new DomainException(
                "Refresh token expiration date must be in the future");

        return new UserRefreshToken(
            userId,
            tokenHash,
            expiresAt,
            createdByIp,
            userAgent);
    }


    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }


    public bool IsRevoked()
    {
        return RevokedAt.HasValue;
    }


    public bool IsActive()
    {
        return !IsRevoked() && !IsExpired();
    }


    public void Revoke(
        string? revokedByIp = null,
        Guid? replacedByTokenId = null)
    {
        if (IsRevoked())
            throw new DomainException(
                "Refresh token is already revoked");

        RevokedAt = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
        ReplacedByTokenId = replacedByTokenId;
    }
}
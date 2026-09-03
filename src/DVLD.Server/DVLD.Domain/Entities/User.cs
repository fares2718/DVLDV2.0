using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

public class User
{
    public Guid UserId { get; private set; }
    public Guid PersonId { get; private set; }

    public string Username { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;

    public bool IsActive { get; private set; }
    public bool IsLocked { get; private set; }

    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockedUntil { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? PasswordChangedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF Core
    private User() { }

    private User(
        Guid personId,
        string username,
        string passwordHash)
    {
        PersonId = personId;
        Username = username;
        PasswordHash = passwordHash;

        IsActive = true;
        IsLocked = false;
        FailedLoginAttempts = 0;
        PasswordChangedAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(
        Guid personId,
        string username,
        string passwordHash)
    {
        if (personId == Guid.Empty)
            throw new DomainException("Person ID cannot be empty");

        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty");

        return new User(
            personId,
            username.Trim(),
            passwordHash);
    }

    public void ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty");

        PasswordHash = passwordHash;
        PasswordChangedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordSuccessfulLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        FailedLoginAttempts = 0;
        IsLocked = false;
        LockedUntil = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Lock(DateTime? lockedUntil = null)
    {
        IsLocked = true;
        LockedUntil = lockedUntil;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        IsLocked = false;
        LockedUntil = null;
        FailedLoginAttempts = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("User is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("User is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty");

        Username = username.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}
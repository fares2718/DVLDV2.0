namespace DVLD.Domain.Views;

public sealed class UserDetailsView
{
    // =====================================================
    // User
    // =====================================================

    public Guid UserId { get; private set; }

    public Guid PersonId { get; private set; }

    public string Username { get; private set; } = null!;

    public bool IsUserActive { get; private set; }

    public bool IsLocked { get; private set; }

    public int FailedLoginAttempts { get; private set; }

    public DateTime? LockedUntil { get; private set; }

    public DateTime? LastLoginAt { get; private set; }

    public DateTime? PasswordChangedAt { get; private set; }

    public DateTime UserCreatedAt { get; private set; }

    public DateTime? UserUpdatedAt { get; private set; }


    // =====================================================
    // Person
    // =====================================================

    public string NationalId { get; private set; } = null!;

    public string FirstName { get; private set; } = null!;

    public string SecondName { get; private set; } = null!;

    public string? ThirdName { get; private set; }

    public string LastName { get; private set; } = null!;

    public string FullName { get; private set; } = null!;

    public string MotherName { get; private set; } = null!;

    public DateOnly DateOfBirth { get; private set; }

    public string Phone { get; private set; } = null!;

    public string? AltPhone { get; private set; }

    public bool Gender { get; private set; }

    public string Email { get; private set; } = null!;

    public string NationalityCountryCode { get; private set; } = null!;

    public string? ImagePath { get; private set; }

    public bool IsPersonActive { get; private set; }

    public DateTime PersonCreatedAt { get; private set; }


    // =====================================================
    // Roles
    // =====================================================

    public string? Roles { get; private set; }
}
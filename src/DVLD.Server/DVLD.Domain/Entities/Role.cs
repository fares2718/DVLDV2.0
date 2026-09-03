using DVLD.Domain.Common;

namespace DVLD.Domain.Entities;

[Flags]
public enum Permission : long
{
    None = 0,

    // People
    ViewPeople      = 1L << 0,
    CreatePeople    = 1L << 1,
    EditPeople      = 1L << 2,

    // Applications
    ViewApplications   = 1L << 3,
    CreateApplications = 1L << 4,
    EditApplications   = 1L << 5,
    CancelApplications = 1L << 6,

    // Licenses
    ViewLicenses      = 1L << 7,
    IssueLicenses     = 1L << 8,
    RenewLicenses     = 1L << 9,
    ReplaceLicenses   = 1L << 10,

    // Tests
    ViewTests       = 1L << 11,
    ScheduleTests   = 1L << 12,
    RecordTestResults = 1L << 13,

    // Detention
    ViewDetentions   = 1L << 14,
    DetainLicenses   = 1L << 15,
    ReleaseLicenses  = 1L << 16,
    ManageFines      = 1L << 17,

    // Users & Roles
    ViewUsers    = 1L << 18,
    ManageUsers  = 1L << 19,
    ViewRoles    = 1L << 20,
    ManageRoles  = 1L << 21,

    // Configuration / Master Data
    ViewMasterData   = 1L << 22,
    ManageMasterData = 1L << 23,

    // Audit
    ViewAuditLogs = 1L << 24,

    // Driver self-service - Phase 2
    ViewOwnData         = 1L << 25,
    SubmitApplications  = 1L << 26,
    UploadDocuments     = 1L << 27,
    TrackOwnApplications = 1L << 28
}

public class Role
{
    public int RoleId { get; private set; }

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public long Permissions { get; private set; }

    public bool IsSystemRole { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    // For EF Core
    private Role()
    {
    }

    private Role(
        string name,
        string? description,
        long permissions,
        bool isSystemRole)
    {
        Name = name;
        Description = NormalizeOptional(description);
        Permissions = permissions;
        IsSystemRole = isSystemRole;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Role Create(
        string name,
        string? description = null,
        long permissions = 0,
        bool isSystemRole = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Role Name cannot be empty");

        if (permissions < 0)
            throw new DomainException(
                "Permissions cannot be negative");

        return new Role(
            name.Trim(),
            description,
            permissions,
            isSystemRole);
    }

    public void UpdateInfo(
        string name,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Role Name cannot be empty");

        Name = name.Trim();
        Description = NormalizeOptional(description);
        UpdatedAt = DateTime.UtcNow;
    }

    public void GrantPermission(long permission)
    {
        if (permission <= 0)
            throw new DomainException(
                "Permission must be greater than zero");

        Permissions |= permission;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RevokePermission(long permission)
    {
        if (permission <= 0)
            throw new DomainException(
                "Permission must be greater than zero");

        Permissions &= ~permission;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasPermission(long permission)
    {
        if (permission <= 0)
            return false;

        return (Permissions & permission) == permission;
    }

    public void Deactivate()
    {
        if (IsSystemRole)
            throw new DomainException(
                "System role cannot be deactivated");

        if (!IsActive)
            throw new DomainException(
                "Role is already inactive");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException(
                "Role is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
    
    public void GrantPermission(Permission permission)
    {
        Permissions |= (long)permission;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RevokePermission(Permission permission)
    {
        Permissions &= ~(long)permission;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasPermission(Permission permission)
    {
        return (Permissions & (long)permission) == (long)permission;
    }
}
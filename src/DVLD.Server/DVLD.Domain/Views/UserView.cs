namespace DVLD.Domain.Views;

public sealed class UserView
{
    public Guid UserId { get; private set; }

    public Guid PersonId { get; private set; }

    public string Username { get; private set; } = null!;

    public string FullName { get; private set; } = null!;

    public string NationalId { get; private set; } = null!;

    public string Phone { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public bool IsLocked { get; private set; }

    public string? Roles { get; private set; }
}
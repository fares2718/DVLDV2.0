namespace DVLD.Domain.Views;

public sealed class AuthenticationUserView
{
    public Guid UserId { get; private set; }

    public string Username { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public string? Roles { get; private set; }
}
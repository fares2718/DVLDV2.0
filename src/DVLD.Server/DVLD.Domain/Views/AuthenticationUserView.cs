namespace DVLD.Domain.Views;

public sealed class AuthenticationUserView
{
    public Guid UserId { get; private set; }

    public string Username { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public List<string> Roles { get; private set; } = new List<string>();
}
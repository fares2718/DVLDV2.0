namespace DVLD.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
    string Username { get; }
    IReadOnlyList<string> Roles { get; }
}
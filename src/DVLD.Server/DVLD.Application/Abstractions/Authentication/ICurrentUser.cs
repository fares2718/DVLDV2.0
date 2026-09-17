namespace DVLD.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
    string Username { get; }
    List<string> Roles { get; }
    long Permissions { get; }
}
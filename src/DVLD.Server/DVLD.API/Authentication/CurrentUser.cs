using System.Security.Claims;
using DVLD.Application.Abstractions.Authentication;

namespace DVLD.API.Authentication;

public class CurrentUser (IHttpContextAccessor accessor):ICurrentUser
{
    private readonly IHttpContextAccessor _accessor = accessor;

    public Guid UserId =>
        Guid.Parse(
            _accessor.HttpContext!
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);

    public string Username =>
        _accessor.HttpContext!
            .User
            .FindFirstValue(ClaimTypes.Email)!;

    public IReadOnlyList<string> Roles =>
        _accessor.HttpContext!
            .User
            .FindAll(ClaimTypes.Role)
            .Select(x => x.Value)
            .ToArray();
}
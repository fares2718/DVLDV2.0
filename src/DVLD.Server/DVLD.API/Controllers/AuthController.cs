using DVLD.API.Authentication;
using DVLD.Application.Features.Authentication.Login;
using DVLD.Application.Features.Authentication.Logout;
using DVLD.Contract.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

public class AuthController(ISender sender,IHttpContextAccessor accessor) : BaseController(sender,accessor)
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> Login(LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var createdByIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers.UserAgent;

        var cmd = new LoginCommand(request.Username,request.Password,createdByIp,userAgent);
        
        var result = await Sender.Send(cmd, cancellationToken);
        if(result.IsError)
            return HandleErrors(result.Errors);
        
        Response.Cookies.Append("token", result.Value.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddMinutes(60)
        });
        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddHours(8)
        });
        
        return Ok("Login success");
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        if (Request.Cookies.TryGetValue("token", out string? token))
        {
            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type is System.Security.Claims.ClaimTypes.NameIdentifier or "sub");
                if (userIdClaim is null || string.IsNullOrEmpty(userIdClaim.Value))
                    return Ok(); //ignore telling for security reasons 

                var userId = Guid.Parse(userIdClaim.Value);
                var revokedByIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

                var cmd = new LogoutCommand(userId, revokedByIp);
                await Sender.Send(cmd, cancellationToken);
            }
            catch
            {
                // Fail silently on token parsing issues during logout 
                // to ensure client cookies still get cleaned up below
            }
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        Response.Cookies.Delete("token", cookieOptions);
        Response.Cookies.Delete("refreshToken", cookieOptions);
        return Ok("Logout success");
    } 
    
    [HttpGet("current-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    
    public IActionResult GetCurrentUser()
    {
        if(!Request.Cookies.TryGetValue("token", out string? token) || string.IsNullOrEmpty(token))
            return Unauthorized("You are not authorized");
        
        var currentUser = new CurrentUser(HttpContextAccessor);
        return Ok(currentUser);
    }
}
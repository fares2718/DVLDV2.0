using DVLD.Application.Features.Authentication.Login;
using DVLD.Contract.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.API.Controllers;

public class AuthController(ISender sender) : BaseController(sender)
{

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
    
}
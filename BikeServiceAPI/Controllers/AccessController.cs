using System.Security.Claims;
using System.Text;
using BikeServiceAPI.Auth;
using BikeServiceAPI.Enums;
using BikeServiceAPI.Models.DTOs;
using BikeServiceAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BikeServiceAPI.Controllers;

[ApiController, Route("[controller]")]
public class AccessController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAccessUtilities _accessUtilities;

    public AccessController(IUserService userService, IAccessUtilities accessUtilities)
    {
        _userService = userService;
        _accessUtilities = accessUtilities;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserDto user)
    {
        user.Roles.Add(Role.StandardUser.ToString());
        await _userService.AddUser(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser()
    {
        string authorizationHeader = HttpContext.Request.Headers["Authorization"];
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeader));
        var parts = credentials.Split(':');
        var encodedName = parts[0];
        var encodedPassword = parts[1];
        var user = await _userService.GetUserByName(encodedName);
        var authenticated = await _accessUtilities.AuthenticateUser(user, encodedPassword);
        if (authenticated)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
            };
            foreach (var role in user.Roles)
            {
                var roleName = Enum.GetName(typeof(Role), role);
                claims.Add(new Claim(ClaimTypes.Role,
                    roleName ?? throw new InvalidOperationException("invalid role enum.")));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                authProperties);
            return Ok(new UserDto(user));
        }

        return Unauthorized("Login Failed");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutUser()
    {
        try
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("User logged out");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Logout failed: {e.Message}");
        }
    }
}

using System.Security.Claims;
using System.Text;
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

    public AccessController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] UserDto user)
    {
        user.Password = _userService.HashPassword(user.Password);
        user.Roles.Add(Role.StandardUser.ToString());
        _userService.AddUser(user);
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
        var authenticated = await _userService.AuthenticateUser(encodedName, encodedPassword);
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
            return Ok("Login Success");
        }

        return Unauthorized("Login Failed");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutUser()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("User logged out");
    }
}
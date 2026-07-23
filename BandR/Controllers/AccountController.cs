using BandR.DTOs.Account;
using BandR.Entities;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BandR.Configuration;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(
    UserManager<ApplicationUser> userManager,
    IJwtService jwtService,
    IAccountService accountService,
    IOptions<JwtConfiguration> jwtConfiguration
) : Controller
{
    private const string RefreshTokenCookieName = "refresh_token";

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AccessTokenResult>> RegisterUser(RegisterDto registerDto, CancellationToken ct)
    {
        ApplicationUser user = new ApplicationUser()
        {
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        IdentityResult result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }

        // var addRoleResult = await userManager.AddToRoleAsync(user, registerDto.Role.ToString());
        // if (!addRoleResult.Succeeded)
        // {
        //     var errors = addRoleResult.Errors.Select(e => e.Description);
        //     return BadRequest(new { Errors = errors });
        // }

        var response = await jwtService.CreateAuthTokenAsync(user, CancellationToken.None);

        SetRefreshTokenCookie(response.RefreshToken);
        return Ok(ToAccessTokenResult(response));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AccessTokenResult>> LoginUser(LoginDto loginDto, CancellationToken ct)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user is null || user.DeactivatedAt is not null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized("Invalid email or password");
        }

        var response = await jwtService.CreateAuthTokenAsync(user, ct);
        SetRefreshTokenCookie(response.RefreshToken);
        return Ok(ToAccessTokenResult(response));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<AccessTokenResult>> RefreshTokenAsync(CancellationToken ct)
    {
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
            return Unauthorized("Invalid refresh token");

        var result = await jwtService.RefreshTokenAsync(refreshToken, ct);
        if (result is null)
        {
            DeleteRefreshTokenCookie();
            return Unauthorized("Invalid refresh token");
        }

        SetRefreshTokenCookie(result.RefreshToken);
        return Ok(ToAccessTokenResult(result));
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        if (Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
            await jwtService.RevokeTokenAsync(refreshToken, ct);

        DeleteRefreshTokenCookie();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeactivateMyAccount(CancellationToken ct)
    {
        await accountService.DeactivateAccountAsync(User.GetUserId(), ct);
        DeleteRefreshTokenCookie();
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        return Ok(await userManager.FindByEmailAsync(email) != null);
    }

    private AccessTokenResult ToAccessTokenResult(AuthTokenResult result) =>
        new(result.AccessToken, result.ExpiresAt);

    private void SetRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/api/account",
            Expires = DateTimeOffset.UtcNow.AddDays(jwtConfiguration.Value.RefreshExpiryDays)
        });
    }

    private void DeleteRefreshTokenCookie() =>
        Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/api/account"
        });
}

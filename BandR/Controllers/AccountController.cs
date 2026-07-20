using BandR.DTOs.Account;
using BandR.Entities;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(
    UserManager<ApplicationUser> userManager,
    IJwtService jwtService,
    IAccountService accountService
) : Controller
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthTokenResult>> RegisterUser(RegisterDto registerDto, CancellationToken ct)
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

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthTokenResult>> LoginUser(LoginDto loginDto, CancellationToken ct)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user is null || user.DeactivatedAt is not null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized("Invalid email or password");
        }

        var response = await jwtService.CreateAuthTokenAsync(user, ct);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthTokenResult>> RefreshTokenAsync(RefreshTokenDto refreshToken, CancellationToken ct)
    {
        var result = await jwtService.RefreshTokenAsync(refreshToken.RefreshToken, ct);
        if (result is null)
        {
            return Unauthorized("Invalid refresh token");
        }
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenDto dto, CancellationToken ct)
    {
        await jwtService.RevokeTokenAsync(dto.RefreshToken, ct);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeactivateMyAccount(CancellationToken ct)
    {
        await accountService.DeactivateAccountAsync(User.GetUserId(), ct);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        return Ok(await userManager.FindByEmailAsync(email) != null);
    }
}

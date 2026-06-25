// using BandR.DTOs.Account;
// using BandR.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
//
// namespace BandR.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// [AllowAnonymous]
// public class AccountController(
//     UserManager<ApplicationUser> userManager,
//     SignInManager<ApplicationUser> signInManager,
//     RoleManager<ApplicationRole> roleManager
// ) : Controller
// {
//     [HttpPost("register")]
//     public async Task<ActionResult<AuthenticationDto>> RegisterUser(RegisterDto registerDto)
//     {
//         ApplicationUser user = new ApplicationUser()
//         {
//             Email = registerDto.Email,
//             UserName = registerDto.Email
//         };
//
//         IdentityResult result = await userManager.CreateAsync(user, registerDto.Password);
//
//         if (!result.Succeeded)
//         {
//             var errors = result.Errors.Select(e => e.Description);
//             return BadRequest(new { Errors = errors });
//         }
//
//         // var addRoleResult = await userManager.AddToRoleAsync(user, registerDto.Role.ToString());
//         // if (!addRoleResult.Succeeded)
//         // {
//         //     var errors = addRoleResult.Errors.Select(e => e.Description);
//         //     return BadRequest(new { Errors = errors });
//         // }
//
//         await signInManager.SignInAsync(user, isPersistent: false);
//
//         var response = await _jwtService.CreateJwtToken(user);
//
//         return Ok(response);
//     }
//     
//     [HttpGet]
//     public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
//     {
//         return Ok(await userManager.FindByEmailAsync(email) != null);
//     }
// }
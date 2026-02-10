using System.Web;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Auth.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "admin")]
public class UsersController(
    UserManager<ApplicationUser> userManager,
    IEmailSender<ApplicationUser> emailSender,
    IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult<IEnumerable<ReadUserDto>> Get()
    {
        var users = userManager.Users
            .Select(u => new ReadUserDto { Id = u.Id, UserName = u.UserName, Email = u.Email })
            .ToList();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserRegistrationResponseDto>> Post([FromBody] UserRegistrationDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Generate the password reset token
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));
        
        // Note: The Auth project's URL is where the Identity UI lives
        var authBaseUrl = configuration["OpenIddict:Issuer"]?.TrimEnd('/'); 
        var callbackUrl = $"{authBaseUrl}/Identity/Account/ResetPassword?code={encodedToken}&email={HttpUtility.UrlEncode(user.Email)}";
        
        await emailSender.SendPasswordResetLinkAsync(user, user.Email!, callbackUrl);

        return Ok(new UserRegistrationResponseDto
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!
        });
    }

    [HttpGet("whoami")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public IActionResult WhoAmI()
    {
        return Ok(new {
            Claims = User.Claims.Select(c => new { c.Type, c.Value }),
            Headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
        });
    }

    [Route("[action]/{id:int}")]
    [HttpPost]
    public ActionResult<string> GetByIds(int id)
    {
        return Ok("hello" + id);
    }
}
using System.Web;
using API.Models;
using Auth;
using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(
    UserManager<ApplicationUser> userManager,
    IEmailSender<ApplicationUser> emailSender,
    IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult<IEnumerable<UserDto>> Get()
    {
        return Ok(userManager.Users
            .Select(u => new UserDto { Id = u.Id, UserName = u.UserName, Email = u.Email }));
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
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
        
        // Note: The Auth project's URL is where the Identity UI lives
        var authBaseUrl = configuration["OpenIddict:Issuer"]?.TrimEnd('/'); 
        var callbackUrl = $"{authBaseUrl}/Identity/Account/ResetPassword?code={HttpUtility.UrlEncode(token)}&email={HttpUtility.UrlEncode(user.Email)}";

        /*TODO fix the password reset link. right now it's sending up invalid token for no reason*/
        await emailSender.SendPasswordResetLinkAsync(user, user.Email!, callbackUrl);

        return Ok(new UserRegistrationResponseDto
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!
        });
    }

    [HttpGet("whoami")]
    [Authorize]
    public IActionResult WhoAmI()
    {
        return Ok(new {
            Claims = User.Claims.Select(c => new { c.Type, c.Value }),
            Headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
        });
    }

    [Route("[action]/{id:int}")]
    [HttpPost]
    [Authorize(Roles = "admin")]
    public ActionResult<string> GetByIds(int id)
    {
        return Ok("hello" + id);
    }
}
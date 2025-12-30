using API.Models;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult<IEnumerable<UserDto>> Get()
    {
        return Ok(_userManager.Users
            .Select(u => new UserDto { Id = u.Id, UserName = u.UserName, Email = u.Email }));
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
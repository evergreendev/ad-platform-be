using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ad_platform.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() => "Hello World!";

    [Route("[action]/{id:int}")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<string> GetByIds(int id)
    {
        return Ok("hello"+id);
    }
        
    
}
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
/*[Authorize] todo add back in after testing*/
public class CampaignsController(): ControllerBase
{
    [HttpPost]
    public IActionResult CreateCampaign(Campaign campaign)
    {
        return Ok();
    }
    
    [HttpGet]
    public IActionResult GetCampaigns()
    {
        return Ok();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetCampaignById(int id)
    {
        return Ok();
    }
}
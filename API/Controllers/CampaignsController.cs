using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CampaignsController(ICampaignService campaignService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCampaign(Campaign campaign)
    {
        var createdCampaign = await campaignService.CreateCampaignAsync(campaign);
        return CreatedAtAction(nameof(GetCampaignById), new { id = createdCampaign.Id }, createdCampaign);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCampaigns()
    {
        var campaigns = await campaignService.GetCampaignsAsync();
        return Ok(campaigns);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCampaignById(Guid id)
    {
        var campaign = await campaignService.GetCampaignByIdAsync(id);
        if (campaign == null)
        {
            return NotFound();
        }
        
        return Ok(campaign);
    }
}
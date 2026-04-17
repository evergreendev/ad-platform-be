using API.DTOs.Campaigns;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
/*[Authorize]*/
public class CampaignsController(ICampaignService campaignService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CampaignResponse>> CreateCampaign(Campaign campaign)
    {
        var createdCampaign = await campaignService.CreateCampaignAsync(campaign);
        return CreatedAtAction(nameof(GetCampaignById), new { id = createdCampaign.Id }, createdCampaign);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CampaignResponse>>> GetCampaigns()
    {
        var campaigns = await campaignService.GetCampaignsAsync();
        return Ok(campaigns);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CampaignResponse>> GetCampaignById(Guid id)
    {
        var campaign = await campaignService.GetCampaignByIdAsync(id);
        if (campaign == null)
        {
            return NotFound();
        }
        
        return Ok(campaign);
    }

    [HttpPost("{id}/contacts")]
    public async Task<IActionResult> AddContacts(Guid id, [FromBody] IEnumerable<Guid> contactIds)
    {
        try
        {
            await campaignService.AddContactsToCampaignAsync(id, contactIds);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
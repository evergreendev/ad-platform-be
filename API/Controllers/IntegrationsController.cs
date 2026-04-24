using API.DTOs.Integrations;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IntegrationsController(IIntegrationService integrationService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<IntegrationResponse>> CreateIntegration(CreateIntegrationRequest request)
    {
        var createdIntegration = await integrationService.CreateIntegrationAsync(request);
        return CreatedAtAction(nameof(GetIntegrationById), new { id = createdIntegration.Id }, createdIntegration);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IntegrationResponse>> GetIntegrationById(Guid id)
    {
        var integration = await integrationService.GetIntegrationByIdAsync(id);
        if (integration == null)
        {
            return NotFound();
        }

        return Ok(integration);
    }
}
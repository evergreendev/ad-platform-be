using API.DTOs.EmailMarketing;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmailMarketingController(IEmailMarketingService emailMarketingService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<ActionResult<SendEmailResponse>> SendEmail([FromBody] SendEmailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await emailMarketingService.SendEmailAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
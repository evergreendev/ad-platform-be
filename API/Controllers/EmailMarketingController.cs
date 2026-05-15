using API.DTOs.EmailMarketing;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EmailMarketingController(IEmailMarketingService emailMarketingService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<ActionResult<SendEmailResponse>> SendEmail([FromBody] SendEmailRequest request, CancellationToken cancellationToken)
    {
        return await ExecuteEmailAction(() => emailMarketingService.SendEmailAsync(request, cancellationToken));
    }

    [HttpPost("schedule")]
    public async Task<ActionResult<SendEmailResponse>> ScheduleEmail([FromBody] ScheduleEmailRequest request, CancellationToken cancellationToken)
    {
        return await ExecuteEmailAction(() => emailMarketingService.ScheduleEmailAsync(request, cancellationToken));
    }

    [HttpPost("{emailMessageId:guid}/reschedule")]
    public async Task<ActionResult<SendEmailResponse>> RescheduleEmail(Guid emailMessageId, [FromBody] RescheduleEmailRequest request, CancellationToken cancellationToken)
    {
        return await ExecuteEmailAction(() => emailMarketingService.RescheduleEmailAsync(emailMessageId, request.ScheduledFor, cancellationToken));
    }

    [HttpPost("{emailMessageId:guid}/cancel")]
    public async Task<ActionResult> CancelScheduledEmail(Guid emailMessageId, CancellationToken cancellationToken)
    {
        try
        {
            await emailMarketingService.CancelScheduledEmailAsync(emailMessageId, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private async Task<ActionResult<SendEmailResponse>> ExecuteEmailAction(Func<Task<SendEmailResponse>> action)
    {
        try
        {
            var result = await action();
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

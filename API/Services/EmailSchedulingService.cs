using API.DTOs.EmailMarketing;
using API.Enums;
using API.Models;

namespace API.Services;

public class EmailSchedulingService(
    IEmailMessageService emailMessageService,
    IEmailJobScheduler emailJobScheduler) : IEmailSchedulingService
{
    public async Task<SendEmailResponse> ScheduleAsync(ScheduleEmailRequest request, CancellationToken cancellationToken = default)
    {
        ValidateScheduledFor(request.ScheduledFor);

        var now = DateTimeOffset.UtcNow;
        var scheduledForUtc = request.ScheduledFor.ToUniversalTime();
        var emailMessage = await emailMessageService.CreateEmailMessage(new EmailMessage
        {
            CampaignId = request.CampaignId,
            ContactId = request.ContactId,
            CompanyContactId = request.CompanyContactId,
            MailMergeTemplateId = request.MailMergeTemplateId,
            ToEmailAddress = request.ToEmail,
            ToDisplayName = request.ToName,
            Subject = request.Subject,
            BodyHtml = request.HtmlBody,
            PlainText = request.PlainTextBody,
            Status = EmailMessageStatus.Scheduled,
            ScheduledFor = scheduledForUtc,
            CreatedAt = now,
            UpdatedAt = now
        }, cancellationToken);

        emailMessage.SchedulerJobId = emailJobScheduler.Schedule(emailMessage.Id, scheduledForUtc);
        await emailMessageService.UpdateEmailMessage(emailMessage, cancellationToken);

        return new SendEmailResponse
        {
            Success = true,
            EmailMessageId = emailMessage.Id,
            Provider = "Internal",
            Message = "Email scheduled.",
            ProviderStatus = emailMessage.Status.ToString(),
            ScheduledFor = emailMessage.ScheduledFor
        };
    }

    public async Task<SendEmailResponse> RescheduleAsync(Guid emailMessageId, DateTimeOffset scheduledFor, CancellationToken cancellationToken = default)
    {
        ValidateScheduledFor(scheduledFor);

        var emailMessage = await emailMessageService.GetEmailMessage(emailMessageId, cancellationToken)
            ?? throw new ArgumentException("Email message not found.");

        if (emailMessage.Status != EmailMessageStatus.Scheduled)
        {
            throw new ArgumentException("Only scheduled emails can be rescheduled.");
        }

        if (!string.IsNullOrWhiteSpace(emailMessage.SchedulerJobId))
        {
            emailJobScheduler.Delete(emailMessage.SchedulerJobId);
        }

        var scheduledForUtc = scheduledFor.ToUniversalTime();
        emailMessage.ScheduledFor = scheduledForUtc;
        emailMessage.SchedulerJobId = emailJobScheduler.Schedule(emailMessage.Id, scheduledForUtc);
        await emailMessageService.UpdateEmailMessage(emailMessage, cancellationToken);

        return new SendEmailResponse
        {
            Success = true,
            EmailMessageId = emailMessage.Id,
            Provider = "Internal",
            Message = "Email rescheduled.",
            ProviderStatus = emailMessage.Status.ToString(),
            ScheduledFor = emailMessage.ScheduledFor
        };
    }

    public async Task CancelAsync(Guid emailMessageId, CancellationToken cancellationToken = default)
    {
        var emailMessage = await emailMessageService.GetEmailMessage(emailMessageId, cancellationToken)
            ?? throw new ArgumentException("Email message not found.");

        if (emailMessage.Status != EmailMessageStatus.Scheduled)
        {
            throw new ArgumentException("Only scheduled emails can be cancelled.");
        }

        if (!string.IsNullOrWhiteSpace(emailMessage.SchedulerJobId))
        {
            emailJobScheduler.Delete(emailMessage.SchedulerJobId);
        }

        emailMessage.Status = EmailMessageStatus.Cancelled;
        emailMessage.CancelledAt = DateTimeOffset.UtcNow;
        await emailMessageService.UpdateEmailMessage(emailMessage, cancellationToken);
    }

    private static void ValidateScheduledFor(DateTimeOffset scheduledFor)
    {
        if (scheduledFor <= DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("ScheduledFor must be in the future.");
        }
    }
}

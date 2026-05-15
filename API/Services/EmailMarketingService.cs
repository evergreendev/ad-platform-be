using API.DTOs.EmailMarketing;
using API.Enums;
using API.Models;

namespace API.Services;

public class EmailMarketingService(
    IEmailMessageService emailMessageService,
    IEmailDeliveryService emailDeliveryService,
    IEmailSchedulingService emailSchedulingService) : IEmailMarketingService
{
    public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
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
            Status = EmailMessageStatus.Queued,
            QueuedAt = now,
            CreatedAt = now,
            UpdatedAt = now
        }, cancellationToken);

        return await emailDeliveryService.SendNowAsync(emailMessage.Id, cancellationToken);
    }

    public Task<SendEmailResponse> ScheduleEmailAsync(ScheduleEmailRequest request, CancellationToken cancellationToken = default)
    {
        return emailSchedulingService.ScheduleAsync(request, cancellationToken);
    }

    public Task<SendEmailResponse> RescheduleEmailAsync(Guid emailMessageId, DateTimeOffset scheduledFor, CancellationToken cancellationToken = default)
    {
        return emailSchedulingService.RescheduleAsync(emailMessageId, scheduledFor, cancellationToken);
    }

    public Task CancelScheduledEmailAsync(Guid emailMessageId, CancellationToken cancellationToken = default)
    {
        return emailSchedulingService.CancelAsync(emailMessageId, cancellationToken);
    }
}

using API.DTOs.EmailMarketing;
using API.DTOs.Integrations;
using API.Enums;
using API.Models;

namespace API.Services;

public class EmailDeliveryService(
    IIntegrationService integrationService,
    IEmailMessageService emailMessageService,
    IExternalRecordLinkService externalRecordLinkService,
    IEnumerable<IEmailMarketingAdapter> adapters) : IEmailDeliveryService
{
    public async Task<SendEmailResponse> SendNowAsync(Guid emailMessageId, CancellationToken cancellationToken = default)
    {
        var emailMessage = await emailMessageService.GetEmailMessage(emailMessageId, cancellationToken)
            ?? throw new ArgumentException("Email message not found.");

        if (emailMessage.Status == EmailMessageStatus.Cancelled)
        {
            throw new ArgumentException("Cancelled emails cannot be sent.");
        }

        var integration = await GetEmailMarketingIntegration();
        var adapter = GetAdapter(integration.Provider);

        emailMessage.Status = EmailMessageStatus.Sending;
        emailMessage.QueuedAt ??= DateTimeOffset.UtcNow;
        emailMessage.AttemptCount += 1;
        emailMessage.LastError = null;
        await emailMessageService.UpdateEmailMessage(emailMessage, cancellationToken);

        try
        {
            var result = await adapter.SendAsync(new EmailProviderSendRequest(
                emailMessage.ToEmailAddress,
                emailMessage.ToDisplayName,
                emailMessage.Subject,
                emailMessage.BodyHtml,
                emailMessage.PlainText), cancellationToken);

            emailMessage.Provider = integration.Provider;
            emailMessage.ProviderMessageId = result.ProviderMessageId;
            emailMessage.Status = result.Status switch
            {
                "sent" => EmailMessageStatus.Sent,
                "queued" or "scheduled" => EmailMessageStatus.Queued,
                "rejected" or "invalid" => EmailMessageStatus.Failed,
                _ => result.Success ? EmailMessageStatus.Queued : EmailMessageStatus.Failed
            };
            emailMessage.SentAt = emailMessage.Status == EmailMessageStatus.Sent ? DateTimeOffset.UtcNow : null;
            emailMessage.FailedAt = emailMessage.Status == EmailMessageStatus.Failed ? DateTimeOffset.UtcNow : null;
            emailMessage.LastError = emailMessage.Status == EmailMessageStatus.Failed ? $"Provider returned status {result.Status}." : null;
            await emailMessageService.UpdateEmailMessage(emailMessage, cancellationToken);

            if (!string.IsNullOrWhiteSpace(result.ProviderMessageId))
            {
                await CreateExternalRecordLink(emailMessage, integration, result.ProviderMessageId);
            }

            return new SendEmailResponse
            {
                Success = result.Success,
                EmailMessageId = emailMessage.Id,
                Provider = integration.Provider,
                Message = result.Success ? $"Email {result.Status} successfully via {integration.Provider}." : $"{integration.Provider} failed to send: {result.Status}",
                ProviderStatus = result.Status,
                ScheduledFor = emailMessage.ScheduledFor
            };
        }
        catch (Exception ex)
        {
            emailMessage.Status = EmailMessageStatus.Failed;
            emailMessage.FailedAt = DateTimeOffset.UtcNow;
            emailMessage.LastError = ex.Message;
            await emailMessageService.UpdateEmailMessage(emailMessage, cancellationToken);
            throw;
        }
    }

    public async Task SendScheduledEmailAsync(Guid emailMessageId)
    {
        var emailMessage = await emailMessageService.GetEmailMessage(emailMessageId);
        if (emailMessage is null || emailMessage.Status != EmailMessageStatus.Scheduled)
        {
            return;
        }

        await SendNowAsync(emailMessageId);
    }

    private async Task<IntegrationResponse> GetEmailMarketingIntegration()
    {
        var integration = await integrationService.GetDefaultIntegrationByCategory(IntegrationCategory.EmailMarketing);
        if (integration == null)
        {
            throw new ArgumentException("Integration not found.");
        }

        if (!integration.IsActive)
        {
            throw new ArgumentException("Integration is inactive.");
        }

        if (integration.Category != IntegrationCategory.EmailMarketing)
        {
            throw new ArgumentException("Integration is not an email marketing integration.");
        }

        return integration;
    }

    private IEmailMarketingAdapter GetAdapter(string provider)
    {
        return adapters.FirstOrDefault(x => string.Equals(x.Provider, provider, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"No email marketing adapter is registered for provider {provider}.");
    }

    private Task CreateExternalRecordLink(EmailMessage emailMessage, IntegrationResponse integration, string providerMessageId)
    {
        return externalRecordLinkService.CreateExternalRecordLink(new ExternalRecordLink
        {
            IntegrationConnectionId = integration.Id,
            InternalEntityType = "email_message",
            InternalEntityId = emailMessage.Id,
            ExternalEntityType = integration.Provider + "_email_message",
            ExternalId = providerMessageId,
            SyncStatus = "synced",
            LastSyncedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        });
    }
}

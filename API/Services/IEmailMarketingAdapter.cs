namespace API.Services;

public interface IEmailMarketingAdapter : IIntegrationAdapter
{
    Task<EmailProviderSendResult> SendAsync(EmailProviderSendRequest request, CancellationToken cancellationToken = default);
}

public record EmailProviderSendRequest(
    string ToEmail,
    string? ToName,
    string Subject,
    string HtmlBody,
    string? PlainTextBody);

public record EmailProviderSendResult(
    bool Success,
    string Status,
    string? ProviderMessageId);

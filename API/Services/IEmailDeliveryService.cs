using API.DTOs.EmailMarketing;

namespace API.Services;

public interface IEmailDeliveryService
{
    Task<SendEmailResponse> SendNowAsync(Guid emailMessageId, CancellationToken cancellationToken = default);
    Task SendScheduledEmailAsync(Guid emailMessageId);
}

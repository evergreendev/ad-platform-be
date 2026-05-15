using API.DTOs.EmailMarketing;

namespace API.Services;

public interface IEmailMarketingService
{
    Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
    Task<SendEmailResponse> ScheduleEmailAsync(ScheduleEmailRequest request, CancellationToken cancellationToken = default);
    Task<SendEmailResponse> RescheduleEmailAsync(Guid emailMessageId, DateTimeOffset scheduledFor, CancellationToken cancellationToken = default);
    Task CancelScheduledEmailAsync(Guid emailMessageId, CancellationToken cancellationToken = default);
}

using API.DTOs.EmailMarketing;

namespace API.Services;

public interface IEmailSchedulingService
{
    Task<SendEmailResponse> ScheduleAsync(ScheduleEmailRequest request, CancellationToken cancellationToken = default);
    Task<SendEmailResponse> RescheduleAsync(Guid emailMessageId, DateTimeOffset scheduledFor, CancellationToken cancellationToken = default);
    Task CancelAsync(Guid emailMessageId, CancellationToken cancellationToken = default);
}

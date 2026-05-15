using Hangfire;

namespace API.Services;

public class HangfireEmailJobScheduler(IBackgroundJobClient backgroundJobClient) : IEmailJobScheduler
{
    public string Schedule(Guid emailMessageId, DateTimeOffset scheduledFor)
    {
        return backgroundJobClient.Schedule<IEmailDeliveryService>(
            service => service.SendScheduledEmailAsync(emailMessageId),
            scheduledFor);
    }

    public bool Delete(string schedulerJobId)
    {
        return backgroundJobClient.Delete(schedulerJobId);
    }
}

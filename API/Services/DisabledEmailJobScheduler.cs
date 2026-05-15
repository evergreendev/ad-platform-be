namespace API.Services;

public class DisabledEmailJobScheduler : IEmailJobScheduler
{
    public string Schedule(Guid emailMessageId, DateTimeOffset scheduledFor)
    {
        throw new InvalidOperationException("Email scheduling requires a Hangfire connection string.");
    }

    public bool Delete(string schedulerJobId)
    {
        return false;
    }
}

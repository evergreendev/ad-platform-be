namespace API.Services;

public interface IEmailJobScheduler
{
    string Schedule(Guid emailMessageId, DateTimeOffset scheduledFor);
    bool Delete(string schedulerJobId);
}

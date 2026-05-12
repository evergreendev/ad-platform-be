namespace API.Models;

public class EmailEvent
{
    public Guid Id { get; set; }

    public Guid EmailMessageId { get; set; }
    public EmailMessage EmailMessage { get; set; } = null!;

    public string EventType { get; set; } = null!;
    public DateTimeOffset OccurredAt { get; set; }
    public DateTimeOffset? ReceivedAt { get; set; }

    public string ProviderEventId { get; set; } = null!;
    public string? MetadataJson { get; set; }
}

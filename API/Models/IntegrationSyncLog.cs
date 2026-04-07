namespace API.Models;

public class IntegrationSyncLog
{
    public Guid Id { get; set; }

    public Guid IntegrationConnectionId { get; set; }
    public IntegrationConnection IntegrationConnection { get; set; } = null!;

    public Guid? ExternalRecordLinkId { get; set; }
    public ExternalRecordLink? ExternalRecordLink { get; set; }

    public string Direction { get; set; } = null!;      // push, pull
    public string Status { get; set; } = null!;         // success, error
    public string? Message { get; set; }

    public string? PayloadJson { get; set; }            // jsonb

    public DateTimeOffset CreatedAt { get; set; }
}
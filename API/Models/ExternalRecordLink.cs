namespace API.Models;

public class ExternalRecordLink
{
    public Guid Id { get; set; }

    public Guid IntegrationConnectionId { get; set; }
    public IntegrationConnection IntegrationConnection { get; set; } = null!;

    public string InternalEntityType { get; set; } = null!;   // contact, company, campaign
    public Guid InternalEntityId { get; set; }

    public string ExternalEntityType { get; set; } = null!;   // member, audience, campaign
    public string ExternalId { get; set; } = null!;

    public string? SyncStatus { get; set; }                   // synced, pending, error
    public DateTimeOffset? LastSyncedAt { get; set; }
    public string? LastSyncError { get; set; }

    public string? ProviderMetadataJson { get; set; }         // jsonb

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<IntegrationSyncLog> SyncLogs { get; set; } = new List<IntegrationSyncLog>();
}
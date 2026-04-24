using API.Enums;

namespace API.Models;

public class IntegrationConnection
{
    public Guid Id { get; set; }
    
    public string Provider { get; set; } = null!;
    public IntegrationCategory Category { get; set; } = IntegrationCategory.EmailMarketing;
    public string DisplayName { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    
    public string? Status { get; set; }
    public string AuthType { get; set; } = "env";
    
    public string? MetadataJson { get; set; }
    public DateTimeOffset? LastSyncedAt { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public ICollection<ExternalRecordLink> ExternalRecordLinks { get; set; } = new List<ExternalRecordLink>();
    public ICollection<IntegrationSyncLog> SyncLogs { get; set; } = new List<IntegrationSyncLog>();
}
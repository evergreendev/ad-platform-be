using API.Enums;

namespace API.Models;

public class CampaignActivityEvent
{
    public Guid Id { get; set; }
    
    public Guid CampaignActivityId { get; set; }
    public CampaignActivity CampaignActivity { get; set; }
    
    public CampaignActivityEventType Type { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
}
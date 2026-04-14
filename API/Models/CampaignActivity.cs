using API.Enums;

namespace API.Models;

public class CampaignActivity
{
    public Guid Id { get; set; }
    
    public Guid CampaignContactId { get; set; }
    public CampaignContact CampaignContact { get; set; }
    
    public CampaignActivityType ActivityType { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public ICollection<CampaignActivityEvent> Events { get; set; } = new List<CampaignActivityEvent>();
}
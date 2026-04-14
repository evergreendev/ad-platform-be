using API.Enums;

namespace API.Models;

public class Campaign
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public CampaignStatus Status { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public ICollection<CampaignContact> CampaignContacts { get; set; } = new List<CampaignContact>();
}
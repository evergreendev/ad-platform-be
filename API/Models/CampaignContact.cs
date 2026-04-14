namespace API.Models;

public class CampaignContact
{
    public Guid Id { get; set; }
    
    public Guid CampaignId { get; set; }
    public Campaign Campaign { get; set; }
    
    public Guid ContactId { get; set; }
    public Contact Contact { get; set; }
    
    public DateTimeOffset AssignedAt { get; set; }
    
    public ICollection<CampaignActivity> Activities { get; set; }
}
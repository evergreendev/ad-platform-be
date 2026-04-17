using API.Enums;

namespace API.DTOs.Campaigns;

public class CampaignResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public CampaignStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<CampaignContactResponse> Contacts { get; set; } = new List<CampaignContactResponse>();
}

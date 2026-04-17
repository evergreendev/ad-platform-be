using API.DTOs;
using API.DTOs.Contacts;

namespace API.DTOs.Campaigns;

public class CampaignContactResponse
{
    public Guid Id { get; set; }
    public Guid CampaignId { get; set; }
    public Guid ContactId { get; set; }
    public ContactResponse? Contact { get; set; }
    public DateTimeOffset AssignedAt { get; set; }
}

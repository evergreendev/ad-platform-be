namespace API.DTOs.Campaigns;

public class AddCompaniesToCampaignRequest
{
    public ICollection<Guid> CompanyIds { get; set; } = new List<Guid>();
    public bool PrimaryContactsOnly { get; set; }
    public ICollection<int> CompanyContactRoleIds { get; set; } = new List<int>();
}

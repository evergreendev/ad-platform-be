using API.Models;

namespace API.Services;

public interface ICampaignService
{
    Task<Campaign> CreateCampaignAsync(Campaign campaign);
    Task<IEnumerable<Campaign>> GetCampaignsAsync();
    Task<Campaign?> GetCampaignByIdAsync(Guid id);
    Task AddContactsToCampaignAsync(Guid campaignId, IEnumerable<Guid> contactIds);
}

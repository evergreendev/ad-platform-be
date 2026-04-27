using API.DTOs.Campaigns;
using API.Models;

namespace API.Services;

public interface ICampaignService
{
    Task<CampaignResponse> CreateCampaignAsync(Campaign campaign);
    Task<IEnumerable<CampaignResponse>> GetCampaignsAsync();
    Task<CampaignResponse?> GetCampaignByIdAsync(Guid id);
    Task AddContactsToCampaignAsync(Guid campaignId, IEnumerable<Guid> contactIds);
    Task AddContactsToCampaignByCompaniesAsync(Guid campaignId, AddCompaniesToCampaignRequest request);
}

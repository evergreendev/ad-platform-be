using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CampaignService(ApplicationDbContext context) : ICampaignService
{
    public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
    {
        campaign.Id = Guid.NewGuid();
        campaign.CreatedAt = DateTimeOffset.UtcNow;
        campaign.UpdatedAt = DateTimeOffset.UtcNow;
        
        context.Campaigns.Add(campaign);
        await context.SaveChangesAsync();
        
        return campaign;
    }

    public async Task<IEnumerable<Campaign>> GetCampaignsAsync()
    {
        return await context.Campaigns
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Campaign?> GetCampaignByIdAsync(Guid id)
    {
        return await context.Campaigns
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddContactsToCampaignAsync(Guid campaignId, IEnumerable<Guid> contactIds)
    {
        var campaign = await context.Campaigns
            .Include(c => c.CampaignContacts)
            .FirstOrDefaultAsync(c => c.Id == campaignId);

        if (campaign == null)
        {
            throw new ArgumentException("Campaign not found", nameof(campaignId));
        }

        var existingContactIds = campaign.CampaignContacts.Select(cc => cc.ContactId).ToHashSet();

        foreach (var contactId in contactIds)
        {
            if (!existingContactIds.Contains(contactId))
            {
                campaign.CampaignContacts.Add(new CampaignContact
                {
                    Id = Guid.NewGuid(),
                    CampaignId = campaignId,
                    ContactId = contactId,
                    AssignedAt = DateTimeOffset.UtcNow
                });
            }
        }

        await context.SaveChangesAsync();
    }
}

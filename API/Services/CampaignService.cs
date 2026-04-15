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
            .Include(c => c.CampaignContacts)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Campaign?> GetCampaignByIdAsync(Guid id)
    {
        return await context.Campaigns
            .Include(c => c.CampaignContacts)
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

        foreach (var contactId in contactIds.Distinct())
        {
            if (existingContactIds.Contains(contactId)) continue;
            campaign.CampaignContacts.Add(new CampaignContact
            {
                CampaignId = campaignId,
                ContactId = contactId,
                AssignedAt = DateTimeOffset.UtcNow
            });
            existingContactIds.Add(contactId);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException { SqlState: Npgsql.PostgresErrorCodes.UniqueViolation })
        {
            // If someone else added the same contacts concurrently, we can ignore the unique violation
            // as the end state (contacts assigned to campaign) is achieved.
        }
    }
}

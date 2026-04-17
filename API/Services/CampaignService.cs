using API.Data;
using API.DTOs;
using API.DTOs.Campaigns;
using API.DTOs.Contacts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CampaignService(ApplicationDbContext context) : ICampaignService
{
    public async Task<CampaignResponse> CreateCampaignAsync(Campaign campaign)
    {
        campaign.Id = Guid.NewGuid();
        campaign.CreatedAt = DateTimeOffset.UtcNow;
        campaign.UpdatedAt = DateTimeOffset.UtcNow;
        
        context.Campaigns.Add(campaign);
        await context.SaveChangesAsync();
        
        return MapToDto(campaign);
    }

    public async Task<IEnumerable<CampaignResponse>> GetCampaignsAsync()
    {
        var campaigns = await context.Campaigns
            .Include(c => c.CampaignContacts)
                .ThenInclude(cc => cc.Contact)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return campaigns.Select(MapToDto);
    }

    public async Task<CampaignResponse?> GetCampaignByIdAsync(Guid id)
    {
        var campaign = await context.Campaigns
            .Include(c => c.CampaignContacts)
                .ThenInclude(cc => cc.Contact)
            .FirstOrDefaultAsync(c => c.Id == id);

        return campaign == null ? null : MapToDto(campaign);
    }

    private static CampaignResponse MapToDto(Campaign campaign)
    {
        return new CampaignResponse
        {
            Id = campaign.Id,
            Name = campaign.Name,
            Description = campaign.Description,
            Status = campaign.Status,
            CreatedAt = campaign.CreatedAt,
            UpdatedAt = campaign.UpdatedAt,
            Contacts = campaign.CampaignContacts.Select(cc => new CampaignContactResponse
            {
                Id = cc.Id,
                CampaignId = cc.CampaignId,
                ContactId = cc.ContactId,
                Contact = new ContactResponse
                {
                    Id = cc.Contact.Id,
                    FirstName = cc.Contact.FirstName,
                    LastName = cc.Contact.LastName,
                    AddressLine1 = cc.Contact.AddressLine1,
                    AddressLine2 = cc.Contact.AddressLine2,
                    City = cc.Contact.City,
                    State = cc.Contact.State,
                    Zip = cc.Contact.Zip,
                    Country = cc.Contact.Country,
                    Salutation = cc.Contact.Salutation,
                    UserRepId = cc.Contact.UserRepId,
                    IsActive = cc.Contact.IsActive,
                    Gender = cc.Contact.Gender,
                    LeadSource = cc.Contact.LeadSource,
                    LeadStatus = cc.Contact.LeadStatus,
                    HubspotId = cc.Contact.HubspotId,
                    JobTitle = cc.Contact.JobTitle,
                    Department = cc.Contact.Department,
                    CreatedDate = cc.Contact.CreatedDate,
                    LastUpdatedDate = cc.Contact.LastUpdatedDate
                },
                AssignedAt = cc.AssignedAt
            }).ToList()
        };
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

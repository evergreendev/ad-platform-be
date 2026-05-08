using API.Data;
using API.DTOs;
using API.DTOs.Campaigns;
using API.DTOs.Contacts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CampaignService(ApplicationDbContext context) : ICampaignService
{
    private const int MaxCampaignsPageSize = 100;
    private const int MaxCampaignContactsPageSize = 100;

    public async Task<CampaignResponse> CreateCampaignAsync(Campaign campaign)
    {
        campaign.Id = Guid.NewGuid();
        campaign.CreatedAt = DateTimeOffset.UtcNow;
        campaign.UpdatedAt = DateTimeOffset.UtcNow;

        context.Campaigns.Add(campaign);
        await context.SaveChangesAsync();

        return MapToDto(campaign);
    }

    public async Task<PagedResponse<CampaignResponse>> GetCampaignsAsync(CampaignsQuery query)
    {
        var page = Math.Max(query.Page ?? 1, 1);
        var pageSize = Math.Clamp(query.PageSize ?? 20, 1, MaxCampaignsPageSize);

        var totalCount = await context.Campaigns.CountAsync();

        var campaigns = await context.Campaigns
            .OrderByDescending(c => c.CreatedAt)
            .ThenBy(c => c.Id)
            .Select(c => new CampaignResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                ContactCount = c.CampaignContacts.Count
            })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<CampaignResponse>
        {
            Items = campaigns,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CampaignResponse?> GetCampaignByIdAsync(Guid id)
    {
        return await context.Campaigns
            .Where(c => c.Id == id)
            .Select(c => new CampaignResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                ContactCount = c.CampaignContacts.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResponse<CampaignContactResponse>?> GetCampaignContactsAsync(
        Guid campaignId,
        CampaignContactsQuery query)
    {
        var campaignExists = await context.Campaigns.AnyAsync(c => c.Id == campaignId);
        if (!campaignExists)
        {
            return null;
        }

        var page = Math.Max(query.Page ?? 1, 1);
        var pageSize = Math.Clamp(query.PageSize ?? 20, 1, MaxCampaignContactsPageSize);

        var dbQuery = context.CampaignContacts
            .AsNoTracking()
            .Where(cc => cc.CampaignId == campaignId);

        var totalCount = await dbQuery.CountAsync();

        var items = await dbQuery
            .Include(cc => cc.Contact)
                .ThenInclude(c => c.Emails)
            .Include(cc => cc.Contact)
                .ThenInclude(c => c.CompanyContacts)
                    .ThenInclude(cc => cc.Company)
            .OrderByDescending(cc => cc.AssignedAt)
            .ThenBy(cc => cc.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<CampaignContactResponse>
        {
            Items = items.Select(MapCampaignContactToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
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

    public async Task AddContactsToCampaignByCompaniesAsync(Guid campaignId, AddCompaniesToCampaignRequest request)
    {
        var companyIds = request.CompanyIds.Distinct().ToList();
        var roleIds = request.CompanyContactRoleIds.Distinct().ToList();

        if (companyIds.Count == 0)
        {
            await AddContactsToCampaignAsync(campaignId, []);
            return;
        }

        var companyContactsQuery = context.CompanyContacts
            .AsNoTracking()
            .Where(cc => companyIds.Contains(cc.CompanyId));

        if (request.PrimaryContactsOnly)
        {
            companyContactsQuery = companyContactsQuery.Where(cc => cc.IsPrimary);
        }

        if (roleIds.Count > 0)
        {
            companyContactsQuery = companyContactsQuery
                .Where(cc => cc.Roles.Any(r => roleIds.Contains(r.RoleId)));
        }

        var contactIds = await companyContactsQuery
            .Select(cc => cc.ContactId)
            .Distinct()
            .ToListAsync();

        await AddContactsToCampaignAsync(campaignId, contactIds);
    }

    public async Task RemoveContactsFromCampaignAsync(Guid campaignId, IEnumerable<Guid> contactIds)
    {
        var campaignExists = await context.Campaigns.AnyAsync(c => c.Id == campaignId);

        if (!campaignExists)
        {
            throw new ArgumentException("Campaign not found", nameof(campaignId));
        }

        var contactIdsToRemove = contactIds.Distinct().ToList();

        if (contactIdsToRemove.Count == 0)
        {
            return;
        }

        var campaignContacts = await context.CampaignContacts
            .Where(cc => cc.CampaignId == campaignId && contactIdsToRemove.Contains(cc.ContactId))
            .ToListAsync();

        context.CampaignContacts.RemoveRange(campaignContacts);
        await context.SaveChangesAsync();
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
            ContactCount = campaign.CampaignContacts.Count
        };
    }

    private static CampaignContactResponse MapCampaignContactToDto(CampaignContact cc)
    {
        return new CampaignContactResponse
        {
            Id = cc.Id,
            CampaignId = cc.CampaignId,
            ContactId = cc.ContactId,
            Contact = MapContactToDto(cc.Contact),
            AssignedAt = cc.AssignedAt
        };
    }

    private static ContactResponse MapContactToDto(Contact contact)
    {
        return new ContactResponse
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            AddressLine1 = contact.AddressLine1,
            AddressLine2 = contact.AddressLine2,
            City = contact.City,
            State = contact.State,
            Zip = contact.Zip,
            Country = contact.Country,
            Salutation = contact.Salutation,
            UserRepId = contact.UserRepId,
            IsActive = contact.IsActive,
            Gender = contact.Gender,
            LeadSource = contact.LeadSource,
            LeadStatus = contact.LeadStatus,
            HubspotId = contact.HubspotId,
            JobTitle = contact.JobTitle,
            Department = contact.Department,
            CreatedDate = contact.CreatedDate,
            LastUpdatedDate = contact.LastUpdatedDate,
            Emails = contact.Emails.Select(e => new ContactEmailResponse
            {
                Id = e.Id,
                Email = e.Email,
                IsPrimary = e.IsPrimary,
                DoNotEmail = e.DoNotEmail
            }).ToList(),
            Companies = contact.CompanyContacts.Select(ccc => new ContactCompanyResponse
            {
                Id = ccc.Id,
                CompanyId = ccc.CompanyId,
                CompanyName = ccc.Company?.CompanyName ?? "Unknown",
                IsPrimary = ccc.IsPrimary,
                Notes = ccc.Notes
            }).ToList()
        };
    }
}

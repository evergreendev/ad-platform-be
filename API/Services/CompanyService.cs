using API.Data;
using API.DTOs;
using API.DTOs.Companies;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CompanyService(ApplicationDbContext context) : ICompanyService
{
    private const int MaxCompanyContactsPageSize = 100;

    public async Task<CompanyResponse> CreateCompanyAsync(CreateCompanyRequest request)
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            CompanyName = request.CompanyName,
            Address = request.Address,
            Address2 = request.Address2,
            City = request.City,
            State = request.State,
            Zip = request.Zip,
            Country = request.Country,
            WebsiteUrl = request.WebsiteUrl,
            Type = request.Type,
            TaxId = request.TaxId,
            Collections = request.Collections,
            WriteOff = request.WriteOff,
            PrimaryRepName = request.PrimaryRepName,
            LegacyPrimaryCategory = request.LegacyPrimaryCategory,
            HubspotCompanyId = request.HubspotCompanyId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsActive = request.IsActive,
            IsNewCompany = request.IsNewCompany,
            CompanySpecialBilling = request.CompanySpecialBilling,
            CreatedDate = DateTimeOffset.UtcNow,
            LastUpdate = DateTimeOffset.UtcNow
        };

        foreach (var contactDto in request.Contacts)
        {
            company.CompanyContacts.Add(new CompanyContact
            {
                Id = Guid.NewGuid(),
                CompanyId = company.Id,
                ContactId = contactDto.ContactId,
                IsPrimary = contactDto.IsPrimary,
                Notes = contactDto.Notes
            });
        }

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        return await GetCompanyByIdAsync(company.Id) ?? throw new Exception("Failed to retrieve created company");
    }

    public async Task<IEnumerable<CompanyResponse>> GetCompaniesAsync()
    {
        return await context.Companies
            .AsNoTracking()
            .OrderBy(c => c.CompanyName)
            .ThenBy(c => c.Id)
            .Select(c => new CompanyResponse
            {
                Id = c.Id,
                CompanyName = c.CompanyName,
                Address = c.Address,
                Address2 = c.Address2,
                City = c.City,
                State = c.State,
                Zip = c.Zip,
                Country = c.Country,
                WebsiteUrl = c.WebsiteUrl,
                Type = c.Type,
                TaxId = c.TaxId,
                LastUpdate = c.LastUpdate,
                Collections = c.Collections,
                WriteOff = c.WriteOff,
                PrimaryRepName = c.PrimaryRepName,
                LegacyPrimaryCategory = c.LegacyPrimaryCategory,
                HubspotCompanyId = c.HubspotCompanyId,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                CreatedDate = c.CreatedDate,
                IsActive = c.IsActive,
                IsNewCompany = c.IsNewCompany,
                CompanySpecialBilling = c.CompanySpecialBilling,
                ContactCount = c.CompanyContacts.Count
            })
            .ToListAsync();
    }

    public async Task<CompanyResponse?> GetCompanyByIdAsync(Guid id)
    {
        return await context.Companies
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CompanyResponse
            {
                Id = c.Id,
                CompanyName = c.CompanyName,
                Address = c.Address,
                Address2 = c.Address2,
                City = c.City,
                State = c.State,
                Zip = c.Zip,
                Country = c.Country,
                WebsiteUrl = c.WebsiteUrl,
                Type = c.Type,
                TaxId = c.TaxId,
                LastUpdate = c.LastUpdate,
                Collections = c.Collections,
                WriteOff = c.WriteOff,
                PrimaryRepName = c.PrimaryRepName,
                LegacyPrimaryCategory = c.LegacyPrimaryCategory,
                HubspotCompanyId = c.HubspotCompanyId,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                CreatedDate = c.CreatedDate,
                IsActive = c.IsActive,
                IsNewCompany = c.IsNewCompany,
                CompanySpecialBilling = c.CompanySpecialBilling,
                ContactCount = c.CompanyContacts.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PagedResponse<CompanyContactResponse>?> GetCompanyContactsAsync(
        Guid companyId,
        CompanyContactsQuery query)
    {
        var companyExists = await context.Companies.AnyAsync(c => c.Id == companyId);
        if (!companyExists)
        {
            return null;
        }

        var page = Math.Max(query.Page ?? 1, 1);
        var pageSize = Math.Clamp(query.PageSize ?? 20, 1, MaxCompanyContactsPageSize);

        var dbQuery = context.CompanyContacts
            .AsNoTracking()
            .Where(cc => cc.CompanyId == companyId);

        var totalCount = await dbQuery.CountAsync();

        var contacts = await dbQuery
            .Include(cc => cc.Contact)
            .OrderByDescending(cc => cc.IsPrimary)
            .ThenBy(cc => cc.Contact.LastName)
            .ThenBy(cc => cc.Contact.FirstName)
            .ThenBy(cc => cc.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<CompanyContactResponse>
        {
            Items = contacts.Select(MapCompanyContactToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    private static CompanyContactResponse MapCompanyContactToDto(CompanyContact companyContact)
    {
        return new CompanyContactResponse
        {
            Id = companyContact.Id,
            ContactId = companyContact.ContactId,
            ContactName = companyContact.Contact != null
                ? $"{companyContact.Contact.FirstName} {companyContact.Contact.LastName}".Trim()
                : "Unknown",
            IsPrimary = companyContact.IsPrimary,
            Notes = companyContact.Notes
        };
    }
}

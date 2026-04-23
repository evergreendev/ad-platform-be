using API.Data;
using API.DTOs.Companies;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CompanyService(ApplicationDbContext context) : ICompanyService
{
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
        var companies = await context.Companies
            .Include(c => c.CompanyContacts)
                .ThenInclude(cc => cc.Contact)
            .ToListAsync();

        return companies.Select(MapToDto);
    }

    public async Task<CompanyResponse?> GetCompanyByIdAsync(Guid id)
    {
        var company = await context.Companies
            .Include(c => c.CompanyContacts)
                .ThenInclude(cc => cc.Contact)
            .FirstOrDefaultAsync(c => c.Id == id);

        return company == null ? null : MapToDto(company);
    }

    private static CompanyResponse MapToDto(Company company)
    {
        return new CompanyResponse
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            Address = company.Address,
            Address2 = company.Address2,
            City = company.City,
            State = company.State,
            Zip = company.Zip,
            Country = company.Country,
            WebsiteUrl = company.WebsiteUrl,
            Type = company.Type,
            TaxId = company.TaxId,
            LastUpdate = company.LastUpdate,
            Collections = company.Collections,
            WriteOff = company.WriteOff,
            PrimaryRepName = company.PrimaryRepName,
            LegacyPrimaryCategory = company.LegacyPrimaryCategory,
            HubspotCompanyId = company.HubspotCompanyId,
            Latitude = company.Latitude,
            Longitude = company.Longitude,
            CreatedDate = company.CreatedDate,
            IsActive = company.IsActive,
            IsNewCompany = company.IsNewCompany,
            CompanySpecialBilling = company.CompanySpecialBilling,
            Contacts = company.CompanyContacts.Select(cc => new CompanyContactResponse
            {
                Id = cc.Id,
                ContactId = cc.ContactId,
                ContactName = cc.Contact != null ? $"{cc.Contact.FirstName} {cc.Contact.LastName}".Trim() : "Unknown",
                IsPrimary = cc.IsPrimary,
                Notes = cc.Notes
            }).ToList()
        };
    }
}

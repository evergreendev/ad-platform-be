using API.Data;
using API.DTOs;
using API.DTOs.Contacts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ContactService(ApplicationDbContext context) : IContactService
{
    private const int MaxContactsPageSize = 100;

    public async Task<ContactResponse> CreateContactAsync(CreateContactRequest request)
    {
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            State = request.State,
            Zip = request.Zip,
            Country = request.Country,
            Salutation = request.Salutation,
            UserRepId = request.UserRepId,
            IsActive = request.IsActive,
            Gender = request.Gender,
            LeadSource = request.LeadSource,
            LeadStatus = request.LeadStatus,
            HubspotId = request.HubspotId,
            JobTitle = request.JobTitle,
            Department = request.Department,
            CreatedDate = DateTimeOffset.UtcNow,
            LastUpdatedDate = DateTimeOffset.UtcNow
        };

        foreach (var emailDto in request.Emails)
        {
            contact.Emails.Add(new ContactEmail
            {
                Id = Guid.NewGuid(),
                Email = emailDto.Email,
                IsPrimary = emailDto.IsPrimary,
                ContactId = contact.Id
            });
        }

        foreach (var companyDto in request.Companies)
        {
            contact.CompanyContacts.Add(new CompanyContact
            {
                Id = Guid.NewGuid(),
                CompanyId = companyDto.CompanyId,
                ContactId = contact.Id,
                IsPrimary = companyDto.IsPrimary,
                Notes = companyDto.Notes
            });
        }

        context.Contacts.Add(contact);
        await context.SaveChangesAsync();

        return await GetContactByIdAsync(contact.Id) ?? throw new Exception("Failed to retrieve created contact");
    }

    public async Task<PagedResponse<ContactResponse>> GetContactsAsync(ContactsQuery query)
    {
        var page = Math.Max(query.Page ?? 1, 1);
        var pageSize = Math.Clamp(query.PageSize ?? 20, 1, MaxContactsPageSize);

        var totalCount = await context.Contacts.CountAsync();

        var contacts = await context.Contacts
            .AsNoTracking()
            .Include(c => c.Emails)
            .Include(c => c.CompanyContacts)
                .ThenInclude(cc => cc.Company)
            .OrderByDescending(c => c.CreatedDate)
            .ThenBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<ContactResponse>
        {
            Items = contacts.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ContactResponse?> GetContactByIdAsync(Guid id)
    {
        var contact = await context.Contacts
            .Include(c => c.Emails)
            .Include(c => c.CompanyContacts)
                .ThenInclude(cc => cc.Company)
            .FirstOrDefaultAsync(c => c.Id == id);

        return contact == null ? null : MapToDto(contact);
    }

    private static ContactResponse MapToDto(Contact contact)
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
            Companies = contact.CompanyContacts.Select(cc => new ContactCompanyResponse
            {
                Id = cc.Id,
                CompanyId = cc.CompanyId,
                CompanyName = cc.Company?.CompanyName ?? "Unknown",
                IsPrimary = cc.IsPrimary,
                Notes = cc.Notes
            }).ToList()
        };
    }
}

namespace API.Models;

public class Contact
{
    public Guid Id { get; set; }
    public ICollection<CompanyContact> CompanyContacts { get; set; } = new List<CompanyContact>();
    public ICollection<ContactEmail> Emails { get; set; } = new List<ContactEmail>();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? Salutation { get; set; }
    /*todo update to be a one to many relation with users*/
    public string? UserRepId { get; set; }
    public bool IsActive { get; set; }
    public string? Gender { get; set; }
    public string? LeadSource { get; set; }
    public string? LeadStatus { get; set; }
    public string? HubspotId { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? LastUpdatedDate { get; set; }
}


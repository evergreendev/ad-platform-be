using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Contacts;

public class CreateContactRequest
{
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? Salutation { get; set; }
    public string? UserRepId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Gender { get; set; }
    public string? LeadSource { get; set; }
    public string? LeadStatus { get; set; }
    public string? HubspotId { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }

    public List<ContactEmailDto> Emails { get; set; } = new();
    public List<ContactCompanyDto> Companies { get; set; } = new();
}

public class ContactEmailDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string? Label { get; set; }
    public bool IsPrimary { get; set; }
}

public class ContactCompanyDto
{
    [Required]
    public Guid CompanyId { get; set; }
    public bool IsPrimary { get; set; }
    public string? Notes { get; set; }
}

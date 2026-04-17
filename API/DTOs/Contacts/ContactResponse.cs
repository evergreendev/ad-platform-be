namespace API.DTOs.Contacts;

public class ContactResponse
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? Salutation { get; set; }
    public string? UserRepId { get; set; }
    public bool IsActive { get; set; }
    public string? Gender { get; set; }
    public string? LeadSource { get; set; }
    public string? LeadStatus { get; set; }
    public string? HubspotId { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}

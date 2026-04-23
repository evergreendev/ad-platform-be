using API.DTOs.Contacts;

namespace API.DTOs.Companies;

public class CompanyResponse
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? Address { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Type { get; set; }
    public string? TaxId { get; set; }
    public DateTimeOffset? LastUpdate { get; set; }
    public bool Collections { get; set; }
    public bool WriteOff { get; set; }
    public string? PrimaryRepName { get; set; }
    public string? LegacyPrimaryCategory { get; set; }
    public long? HubspotCompanyId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsNewCompany { get; set; }
    public bool? CompanySpecialBilling { get; set; }
    
    public ICollection<CompanyContactResponse> Contacts { get; set; } = new List<CompanyContactResponse>();
}

public class CompanyContactResponse
{
    public Guid Id { get; set; }
    public Guid ContactId { get; set; }
    public string ContactName { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public string? Notes { get; set; }
}

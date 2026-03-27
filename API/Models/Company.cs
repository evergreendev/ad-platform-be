using System;

namespace API.Models;

public class Company
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
    /*todo(domain): FK to Category once category is mapped*/
    /*public int? Category { get; set; }*/
    public string? Type { get; set; }
    public string? TaxId { get; set; }
    
    /*todo(domain): FK to currency once currency is mapped
     
    public int CurrencyId { get; set; }*/
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
    
    public ICollection<CompanyContact> CompanyContacts { get; set; } = new List<CompanyContact>();
}

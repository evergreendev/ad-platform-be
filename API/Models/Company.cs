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
    /* todo(domain): FK to billing contact once contacts are mapped
     * 
     * public string? BillingContactId { get; set; }
     */
    /* todo(domain): FK to primary contact once contacts are mapped
     * 
     * public string? PrimaryContactId { get; set; }
     */
    /* todo(domain): FK to print artwork contact once contacts are mapped
     
     * public string? PrintArtworkContactId { get; set; }
    public string? DigitalArtworkContactId { get; set; }*/
    /*todo(domain): FK to primary category once categories mapped*/
    public string? LegacyPrimaryCategory { get; set; }
    public long? HubspotCompanyId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public bool Dead { get; set; }
    public bool IsNewCompany { get; set; }
    public bool? CompanySpecialBilling { get; set; }
}

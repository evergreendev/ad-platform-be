using System;

namespace API.Models;

public record CompanyQuery
{
    public string? CompanyName { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Zip { get; init; }
    public string? Country { get; init; }
    public string? WebsiteUrl { get; init; }
    public string? Type { get; init; }
    public string? TaxId { get; init; }
    public string? PrimaryRepName { get; init; }
    public string? LegacyPrimaryCategory { get; init; }
    public long? HubspotCompanyId { get; init; }
    public bool? Collections { get; init; }
    public bool? WriteOff { get; init; }
    public bool? Dead { get; init; }
    public bool? IsNewCompany { get; init; }
    public bool? CompanySpecialBilling { get; init; }
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 20;
}

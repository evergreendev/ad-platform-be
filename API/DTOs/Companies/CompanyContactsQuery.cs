namespace API.DTOs.Companies;

public record CompanyContactsQuery
{
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 20;
}

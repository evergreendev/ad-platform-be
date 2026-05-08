namespace API.DTOs.Contacts;

public record ContactsQuery
{
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 20;
}

namespace API.DTOs.Campaigns;

public record CampaignContactsQuery
{
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 20;
}

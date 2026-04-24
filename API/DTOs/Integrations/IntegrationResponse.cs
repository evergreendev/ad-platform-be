using API.Enums;

namespace API.DTOs.Integrations;

public class IntegrationResponse
{
    public Guid Id { get; set; }
    public string Provider { get; set; } = null!;
    public IntegrationCategory Category { get; set; }
    public string DisplayName { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? Status { get; set; }
    public string AuthType { get; set; } = null!;
    public string? MetadataJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
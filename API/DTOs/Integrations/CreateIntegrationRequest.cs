using System.ComponentModel.DataAnnotations;
using API.Enums;

namespace API.DTOs.Integrations;

public class CreateIntegrationRequest
{
    [Required]
    public string Provider { get; set; } = null!;

    [Required]
    public IntegrationCategory Category { get; set; } = IntegrationCategory.EmailMarketing;

    [Required]
    public string DisplayName { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public string? Status { get; set; }

    [Required]
    public string AuthType { get; set; } = "env";

    public string? MetadataJson { get; set; }
}
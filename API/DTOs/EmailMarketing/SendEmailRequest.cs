using System.ComponentModel.DataAnnotations;

namespace API.DTOs.EmailMarketing;

public class SendEmailRequest
{
    [Required]
    public Guid IntegrationId { get; set; }

    [Required]
    [EmailAddress]
    public string ToEmail { get; set; } = null!;

    [Required]
    public string Subject { get; set; } = null!;

    [Required]
    public string HtmlBody { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.EmailMarketing;

public class SendEmailRequest
{
    public Guid? CampaignId { get; set; }
    public Guid? ContactId { get; set; }
    public Guid? CompanyContactId { get; set; }
    public Guid? MailMergeTemplateId { get; set; }
    
    [Required]
    [EmailAddress]
    public string ToEmail { get; set; } = null!;

    public string? ToName { get; set; } = null!;

    [Required]
    public string Subject { get; set; } = null!;

    [Required]
    public string HtmlBody { get; set; } = null!;
    
    public string? PlainTextBody { get; set; } = null!;
}
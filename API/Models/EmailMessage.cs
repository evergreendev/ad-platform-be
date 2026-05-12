using API.Enums;

namespace API.Models;

public class EmailMessage
{
    public Guid Id { get; set; }

    public Guid? CampaignId { get; set; }
    public Campaign? Campaign { get; set; }

    public Guid? ContactId { get; set; }
    public Contact? Contact { get; set; }

    public Guid? CompanyContactId { get; set; }
    public CompanyContact? CompanyContact { get; set; }

    public Guid? MailMergeTemplateId { get; set; }
    public MailMergeTemplate? MailMergeTemplate { get; set; }

    public string ToEmailAddress { get; set; } = null!;
    public string? ToDisplayName { get; set; }

    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;
    public string? PlainText { get; set; }

    public EmailMessageStatus Status { get; set; }

    public string? Provider { get; set; }
    public string? ProviderMessageId { get; set; }

    public DateTimeOffset? QueuedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
    public DateTimeOffset? DeliveredAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<EmailEvent> Events { get; set; } = new List<EmailEvent>();
    public ICollection<EmailLink> Links { get; set; } = new List<EmailLink>();
}

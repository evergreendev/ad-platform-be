namespace API.Models;

public class MailMergeTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string SubjectTemplate { get; set; } = null!;
    public string BodyJson { get; set; } = null!;
    public string? BodyHtml { get; set; }
    public string? PlainText { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<MailMergeTemplateField> TemplateFields { get; set; } = new List<MailMergeTemplateField>();
}

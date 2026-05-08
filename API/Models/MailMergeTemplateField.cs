namespace API.Models;

public class MailMergeTemplateField
{
    public Guid TemplateId { get; set; }
    public MailMergeTemplate Template { get; set; } = null!;

    public string MergeFieldKey { get; set; } = null!;
    public MergeField MergeField { get; set; } = null!;
}

namespace API.Models;

public class MergeField
{
    public string Key { get; set; } = null!;
    public string Label { get; set; } = null!;
    public string Entity { get; set; } = null!;
    public string DataType { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<MailMergeTemplateField> TemplateFields { get; set; } = new List<MailMergeTemplateField>();
}

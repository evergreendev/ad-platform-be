namespace API.Models;

public class EmailLink
{
    public Guid Id { get; set; }

    public Guid EmailMessageId { get; set; }
    public EmailMessage EmailMessage { get; set; } = null!;

    public string OriginalUrl { get; set; } = null!;
    public string Label { get; set; } = null!;
}

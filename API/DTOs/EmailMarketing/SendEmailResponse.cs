namespace API.DTOs.EmailMarketing;

public class SendEmailResponse
{
    public bool Success { get; set; }
    public string Provider { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string? ProviderStatus { get; set; }
}
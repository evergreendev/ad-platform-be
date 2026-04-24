using API.DTOs.EmailMarketing;

namespace API.Services;

public interface IEmailMarketingService
{
    Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}
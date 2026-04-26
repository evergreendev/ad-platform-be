using System.Net.Http.Headers;
using System.Text.Json;
using API.DTOs.EmailMarketing;
using API.Enums;

namespace API.Services;

public class EmailMarketingService(IIntegrationService integrationService, IHttpClientFactory httpClientFactory, IConfiguration configuration) : IEmailMarketingService
{
    public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        var integration = await integrationService.GetIntegrationByIdAsync(request.IntegrationId);
        if (integration == null)
        {
            throw new ArgumentException("Integration not found.");
        }

        if (!integration.IsActive)
        {
            throw new ArgumentException("Integration is inactive.");
        }

        if (integration.Category != IntegrationCategory.EmailMarketing)
        {
            throw new ArgumentException("Integration is not an email marketing integration.");
        }

        if (!string.Equals(integration.Provider, "Mailchimp", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Only Mailchimp provider is supported for this endpoint right now.");
        }

        var apiKey = configuration["Integrations:Mailchimp:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Missing configuration key Integrations:Mailchimp:ApiKey.");
        }

        var client = httpClientFactory.CreateClient();
        var sendRequest = new
        {
            key = apiKey,
            message = new
            {
                html = request.HtmlBody,
                subject = request.Subject,
                from_email = configuration["Integrations:Mailchimp:FromEmail"] ?? "noreply@ad-platform.com",
                from_name = configuration["Integrations:Mailchimp:FromName"] ?? "Ad Platform",
                to = new[]
                {
                    new { email = request.ToEmail, type = "to" }
                }
            }
        };

        var response = await client.PostAsJsonAsync("https://mandrillapp.com/api/1.0/messages/send", sendRequest, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Mailchimp API returned error: {response.StatusCode} - {responseBody}");
        }

        using var document = JsonDocument.Parse(responseBody);
        var firstResult = document.RootElement[0];
        var status = firstResult.GetProperty("status").GetString();

        var success = status is "sent" or "queued" or "scheduled";

        return new SendEmailResponse
        {
            Success = success,
            Provider = integration.Provider,
            Message = success ? $"Email {status} successfully via Mailchimp." : $"Mailchimp failed to send: {status}",
            ProviderStatus = status
        };
    }

    private async Task<string?> CheckMailchimpStatusAsync(CancellationToken cancellationToken)
    {
        var apiKey = configuration["Integrations:Mailchimp:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Missing configuration key Integrations:Mailchimp:ApiKey.");
        }

        var client = httpClientFactory.CreateClient();
        var pingRequest = new { key = apiKey };

        using var response = await client.PostAsJsonAsync("https://mandrillapp.com/api/1.0/users/ping", pingRequest, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return $"Error: {response.StatusCode}";
        }

        return responseBody.Trim('\"'); // Returns "PONG!"
    }
}
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

        var providerStatus = await CheckMailchimpStatusAsync(cancellationToken);

        return new SendEmailResponse
        {
            Success = true,
            Provider = integration.Provider,
            Message = "Mailchimp status check succeeded. Placeholder send route is healthy.",
            ProviderStatus = providerStatus
        };
    }

    private async Task<string?> CheckMailchimpStatusAsync(CancellationToken cancellationToken)
    {
        var apiKey = configuration["Integrations:Mailchimp:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Missing configuration key Integrations:Mailchimp:ApiKey.");
        }

        var dataCenter = configuration["Integrations:Mailchimp:DataCenter"];
        if (string.IsNullOrWhiteSpace(dataCenter))
        {
            var keyParts = apiKey.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            dataCenter = keyParts.Length > 1 ? keyParts[^1] : null;
        }

        if (string.IsNullOrWhiteSpace(dataCenter))
        {
            throw new InvalidOperationException("Mailchimp data center is missing. Set Integrations:Mailchimp:DataCenter or use an API key with dc suffix (example: key-us1).");
        }

        var client = httpClientFactory.CreateClient();
        var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"any:{apiKey}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        using var response = await client.GetAsync($"https://{dataCenter}.api.mailchimp.com/3.0/ping", cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(responseBody);
        if (document.RootElement.TryGetProperty("health_status", out var healthStatus))
        {
            return healthStatus.GetString();
        }

        return null;
    }
}
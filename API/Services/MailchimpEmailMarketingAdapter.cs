using System.Text.Json;
using API.Enums;
using API.Models;

namespace API.Services;

public class MailchimpEmailMarketingAdapter(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IEmailMarketingAdapter
{
    public string Provider => "Mailchimp";
    public IntegrationCategory Category => IntegrationCategory.EmailMarketing;

    public async Task ValidateConnectionAsync(IntegrationConnection connection, CancellationToken cancellationToken = default)
    {
        if (!string.Equals(connection.Provider, Provider, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Integration is not a Mailchimp integration.");
        }

        var apiKey = GetApiKey();
        var client = httpClientFactory.CreateClient();
        var pingRequest = new { key = apiKey };

        using var response = await client.PostAsJsonAsync("https://mandrillapp.com/api/1.0/users/ping", pingRequest, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Mailchimp API returned error: {response.StatusCode} - {responseBody}");
        }
    }

    public async Task<EmailProviderSendResult> SendAsync(EmailProviderSendRequest request, CancellationToken cancellationToken = default)
    {
        var apiKey = GetApiKey();
        var client = httpClientFactory.CreateClient();
        var sendRequest = new
        {
            key = apiKey,
            message = new
            {
                html = request.HtmlBody,
                text = request.PlainTextBody,
                subject = request.Subject,
                from_email = configuration["Integrations:Mailchimp:FromEmail"] ?? "joe@egmrc.com",
                from_name = configuration["Integrations:Mailchimp:FromName"] ?? "Ad Platform",
                to = new[]
                {
                    new { email = request.ToEmail, name = request.ToName, type = "to" }
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
        var status = firstResult.GetProperty("status").GetString() ?? "unknown";
        var externalId = firstResult.TryGetProperty("_id", out var idProperty) ? idProperty.GetString() : null;
        var success = status is "sent" or "queued" or "scheduled";

        return new EmailProviderSendResult(success, status, externalId);
    }

    private string GetApiKey()
    {
        var apiKey = configuration["Integrations:Mailchimp:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Missing configuration key Integrations:Mailchimp:ApiKey.");
        }

        return apiKey;
    }
}

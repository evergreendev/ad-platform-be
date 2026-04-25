using API.Data;
using API.DTOs.Integrations;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class IntegrationService(ApplicationDbContext context) : IIntegrationService
{
    public async Task<IntegrationResponse> CreateIntegrationAsync(CreateIntegrationRequest request)
    {
        var existing = await context.IntegrationConnections
            .AnyAsync(x => x.Provider == request.Provider && x.DisplayName == request.DisplayName);

        if (existing)
        {
            throw new InvalidOperationException($"Integration with provider '{request.Provider}' and name '{request.DisplayName}' already exists.");
        }

        var integrationConnection = new IntegrationConnection
        {
            Id = Guid.NewGuid(),
            Provider = request.Provider,
            Category = request.Category,
            DisplayName = request.DisplayName,
            IsActive = request.IsActive,
            Status = request.Status,
            AuthType = request.AuthType,
            MetadataJson = request.MetadataJson,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        context.IntegrationConnections.Add(integrationConnection);
        await context.SaveChangesAsync();

        return await GetIntegrationByIdAsync(integrationConnection.Id)
               ?? throw new Exception("Failed to retrieve created integration");
    }

    public async Task<IntegrationResponse?> GetIntegrationByIdAsync(Guid id)
    {
        var integrationConnection = await context.IntegrationConnections
            .FirstOrDefaultAsync(x => x.Id == id);

        return integrationConnection == null ? null : MapToDto(integrationConnection);
    }

    private static IntegrationResponse MapToDto(IntegrationConnection integrationConnection)
    {
        return new IntegrationResponse
        {
            Id = integrationConnection.Id,
            Provider = integrationConnection.Provider,
            Category = integrationConnection.Category,
            DisplayName = integrationConnection.DisplayName,
            IsActive = integrationConnection.IsActive,
            Status = integrationConnection.Status,
            AuthType = integrationConnection.AuthType,
            MetadataJson = integrationConnection.MetadataJson,
            CreatedAt = integrationConnection.CreatedAt,
            UpdatedAt = integrationConnection.UpdatedAt
        };
    }
}
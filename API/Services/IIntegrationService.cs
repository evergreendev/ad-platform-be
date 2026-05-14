using API.DTOs.Integrations;
using API.Enums;

namespace API.Services;

public interface IIntegrationService
{
    Task<IntegrationResponse> CreateIntegrationAsync(CreateIntegrationRequest request);
    Task<IntegrationResponse?> GetIntegrationByIdAsync(Guid id);
    Task<IntegrationResponse?> GetDefaultIntegrationByCategory(IntegrationCategory category);
}
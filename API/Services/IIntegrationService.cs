using API.DTOs.Integrations;

namespace API.Services;

public interface IIntegrationService
{
    Task<IntegrationResponse> CreateIntegrationAsync(CreateIntegrationRequest request);
    Task<IntegrationResponse?> GetIntegrationByIdAsync(Guid id);
}
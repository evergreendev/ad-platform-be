using API.Enums;
using API.Models;

namespace API.Services;

public interface IIntegrationAdapter
{
    string Provider { get; }
    IntegrationCategory Category { get; }
    Task ValidateConnectionAsync(IntegrationConnection connection, CancellationToken cancellationToken = default);
}

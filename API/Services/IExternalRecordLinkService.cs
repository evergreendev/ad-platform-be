using API.Models;

namespace API.Services;

public interface IExternalRecordLinkService
{
    public Task<ExternalRecordLink> CreateExternalRecordLink(ExternalRecordLink externalRecordLink);
}
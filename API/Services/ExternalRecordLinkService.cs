using API.Data;
using API.Models;

namespace API.Services;

public class ExternalRecordLinkService(ApplicationDbContext context): IExternalRecordLinkService
{
    public Task<ExternalRecordLink> CreateExternalRecordLink(ExternalRecordLink externalRecordLink)
    {
        context.ExternalRecordLinks.Add(externalRecordLink);
        
        context.SaveChanges();
        return Task.FromResult(externalRecordLink);
    }
}
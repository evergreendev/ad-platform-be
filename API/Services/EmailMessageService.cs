using API.Data;
using API.Enums;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class EmailMessageService(ApplicationDbContext context): IEmailMessageService
{
    public async Task<EmailMessage> CreateEmailMessage(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        context.EmailMessages.Add(emailMessage);
        await context.SaveChangesAsync(cancellationToken);
        return emailMessage;
    }

    public Task<EmailMessage?> GetEmailMessage(Guid id, CancellationToken cancellationToken = default)
    {
        return context.EmailMessages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdateEmailMessage(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        emailMessage.UpdatedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateStatus(Guid id, EmailMessageStatus status, CancellationToken cancellationToken = default)
    {
        var emailMessage = await GetEmailMessage(id, cancellationToken)
            ?? throw new ArgumentException("Email message not found.");

        emailMessage.Status = status;
        emailMessage.UpdatedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
    }
}

using API.Data;
using API.Models;

namespace API.Services;

public class EmailMessageService(ApplicationDbContext context): IEmailMessageService
{
    public Task<EmailMessage> CreateEmailMessage(EmailMessage emailMessage)
    {
        context.EmailMessages.Add(emailMessage);
        context.SaveChanges();
        return Task.FromResult(emailMessage);
    }
}
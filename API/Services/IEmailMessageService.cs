using API.Enums;
using API.Models;

namespace API.Services;

public interface IEmailMessageService
{
    Task<EmailMessage> CreateEmailMessage(EmailMessage emailMessage, CancellationToken cancellationToken = default);
    Task<EmailMessage?> GetEmailMessage(Guid id, CancellationToken cancellationToken = default);
    Task UpdateEmailMessage(EmailMessage emailMessage, CancellationToken cancellationToken = default);
    Task UpdateStatus(Guid id, EmailMessageStatus status, CancellationToken cancellationToken = default);
}

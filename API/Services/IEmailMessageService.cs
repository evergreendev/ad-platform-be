using API.Data;
using API.Models;

namespace API.Services;

public interface IEmailMessageService
{
    public Task<EmailMessage> CreateEmailMessage(EmailMessage emailMessage);
}
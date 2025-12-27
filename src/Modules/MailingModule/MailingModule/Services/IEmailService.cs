using MailingModule.Models;

namespace MailingModule.Services;

public interface IEmailService
{
    void SendEmail(Email email);
}
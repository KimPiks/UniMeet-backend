using System.Net;
using System.Net.Mail;
using MailingModule.Config;
using MailingModule.Models;
using Serilog;

namespace MailingModule.Services;

public class EmailService : IEmailService, IDisposable
{
    private readonly Configuration _configuration;
    private readonly SmtpClient _client;
    
    public EmailService(Configuration configuration)
    {
        _configuration = configuration;
        _client = new SmtpClient(configuration.Smtp.Host, configuration.Smtp.Port)
        {
            Credentials = new NetworkCredential(configuration.Smtp.Username, configuration.Smtp.Password),
            EnableSsl = true
        };
    }
    
    public void SendEmail(Email email)
    {
        try
        {
            var message = new MailMessage(_configuration.Smtp.Username, email.To, email.Subject, email.Body)
            {
                IsBodyHtml = true,
                From = new MailAddress(_configuration.Smtp.Username, _configuration.Smtp.SenderName)
            };

            _client.Send(message);
        }
        catch (SmtpFailedRecipientException ex)
        {
            Log.Warning("Could not deliver email to {Recipient}: {Message}", ex.FailedRecipient, ex.Message);
        }
        catch (SmtpException ex)
        {
            Log.Error("SMTP error while sending email: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error occurred");
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
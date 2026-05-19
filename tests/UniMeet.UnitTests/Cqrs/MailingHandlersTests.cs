using MailingModule.Commands;
using MailingModule.Models;
using MailingModule.Services;
using ModularSystem.Contracts.Mailing;
using UniMeet.Shared.Exceptions;

namespace UniMeet.UnitTests.Cqrs;

public class MailingHandlersTests
{
    [Fact]
    public async Task SendEmailCommandHandler_validates_and_sends_email()
    {
        EnsureEmailTemplates();
        var emailService = new CapturingEmailService();
        var handler = new SendEmailCommandHandler(emailService);
        var parameters = new List<EmailParameter> { new("link", "https://example.test") };

        await handler.HandleAsync(new SendEmailCommand("student@uni.edu", EmailType.RegisterConfirmation, parameters));

        Assert.NotNull(emailService.SentEmail);
        Assert.Equal("student@uni.edu", emailService.SentEmail.To);
        Assert.False(string.IsNullOrWhiteSpace(emailService.SentEmail.Subject));
        Assert.DoesNotContain("{{link}}", emailService.SentEmail.Body);
    }

    [Fact]
    public async Task SendEmailCommandHandler_rejects_invalid_recipient()
    {
        var emailService = new CapturingEmailService();
        var handler = new SendEmailCommandHandler(emailService);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.HandleAsync(new SendEmailCommand("bad", EmailType.PasswordReset, [])));

        Assert.Null(emailService.SentEmail);
    }

    private sealed class CapturingEmailService : IEmailService
    {
        public Email? SentEmail { get; private set; }

        public void SendEmail(Email email)
        {
            SentEmail = email;
        }
    }

    private static void EnsureEmailTemplates()
    {
        var directory = Path.Combine(AppContext.BaseDirectory, "EmailTemplates");
        Directory.CreateDirectory(directory);
        File.WriteAllText(Path.Combine(directory, "RegisterConfirmation.html"), "Confirm at {{link}}");
        File.WriteAllText(Path.Combine(directory, "PasswordReset.html"), "Reset at {{link}}");
    }
}

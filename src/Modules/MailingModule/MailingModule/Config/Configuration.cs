namespace MailingModule.Config;

public record Configuration(SmtpConfiguration Smtp);

public record SmtpConfiguration(string Host, int Port, string Username, string Password, string SenderName);
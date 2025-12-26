using MailingModule.Enums;

namespace MailingModule.Models;

public class Email
{
    public Email(string to, EmailType emailType, List<EmailParameter>? parameters)
    {
        To = to;
        Subject = EmailTemplate.GetSubject(emailType);
        Body = EmailTemplate.GetTemplate(emailType);

        if (parameters == null) return;
        Body = EmailTemplate.SetTemplateParameters(Body, parameters);
    }
    
    public string To { get; init; }
    public string Subject { get; init; }
    public string Body { get; init; }
}
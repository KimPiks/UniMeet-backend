using MailingModule.Enums;
using MailingModule.Models;

namespace MailingModule;

public static class EmailTemplate
{
    private static readonly Dictionary<EmailType, string> EmailTemplates = new()
    {
        { EmailType.RegisterConfirmation, "EmailTemplates/RegisterConfirmation.html" },
        { EmailType.PasswordReset, "EmailTemplates/PasswordReset.html" }
    };
    
    private static readonly Dictionary<EmailType, string> EmailSubjects = new()
    {
        { EmailType.RegisterConfirmation, "Welcome to UniMeet" },
        { EmailType.PasswordReset, "Reset your password" }
    };
    
    /// <summary>
    /// Read template file to string
    /// </summary>
    /// <param name="emailType">Email type</param>
    /// <returns>Email template as text</returns>
    public static string GetTemplate(EmailType emailType)
    {
        var path = GetPath(emailType);
        return File.ReadAllText(path);
    }
    
    /// <summary>
    /// Get email subject for email type
    /// </summary>
    /// <param name="emailType">Email type</param>
    /// <returns>Email subject</returns>
    /// <exception cref="ArgumentException">There is no registered subject for email type</exception>
    public static string GetSubject(EmailType emailType)
    {
        if (!EmailSubjects.TryGetValue(emailType, out var subject))
        {
            throw new ArgumentException($"Email subject for {emailType} not found.");
        }
        
        return subject;
    }
    
    /// <summary>
    /// Set parameters in email template
    /// </summary>
    /// <param name="template">Email template</param>
    /// <param name="parameters">List of parameters</param>
    /// <returns>Template with parameters</returns>
    public static string SetTemplateParameters(string template, List<EmailParameter> parameters)
    {
        foreach (var parameter in parameters)
        {
            template = template.Replace(@"{{" + parameter.Key + @"}}", parameter.Value);
        }
        
        return template;
    }
    
    /// <summary>
    /// Get html template path for email type
    /// </summary>
    /// <param name="emailType">Email type</param>
    /// <returns>Email template path string</returns>
    /// <exception cref="ArgumentException">There is no registered template for email type</exception>
    private static string GetPath(EmailType emailType)
    {
        if (!EmailTemplates.TryGetValue(emailType, out var path))
        {
            throw new ArgumentException($"Email template for {emailType} not found.");
        }
        
        return path;
    }
}
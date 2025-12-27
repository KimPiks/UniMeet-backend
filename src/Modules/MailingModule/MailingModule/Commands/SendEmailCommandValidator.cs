using UniMeet.Shared.Exceptions;

namespace MailingModule.Commands;

public static class SendEmailCommandValidator
{
    public static void Validate(this SendEmailCommand request)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(request.To) || request.To.Length < 5 || !request.To.Contains("@"))
            errors.Add("Recipient email address cannot be empty.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
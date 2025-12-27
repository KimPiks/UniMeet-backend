using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Application.PasswordResetCodes.RequestPasswordReset;

public static class RequestPasswordResetCommandValidator
{
    public static void Validate(this RequestPasswordResetCommand request)
    {
        var errors = new List<string>();
     
        if (request.Email.Length < 5 || !request.Email.Contains("@"))
            errors.Add("Email address cannot be empty.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
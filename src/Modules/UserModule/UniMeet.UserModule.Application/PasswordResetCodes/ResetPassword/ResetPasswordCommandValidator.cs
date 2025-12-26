using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Application.PasswordResetCodes.ResetPassword;

public static class ResetPasswordCommandValidator
{
    public static void Validate(this ResetPasswordCommand request)
    {
        var errors = new List<string>();
     
        // TODO: remove duplicated code from here and register command
        if (request.NewPassword.Length < 8)
            errors.Add("Password must be at least 8 characters long.");
        
        if (request.NewPassword.Length > 100)
            errors.Add("Password cannot exceed 100 characters.");
        
        if (!request.NewPassword.Any(char.IsUpper) ||
            !request.NewPassword.Any(char.IsLower) ||
            !request.NewPassword.Any(char.IsDigit))
            errors.Add("Password must contain at least one uppercase letter, one lowercase letter and one digit");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
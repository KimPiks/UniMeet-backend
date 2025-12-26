using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Application.Users.LoginUser;

public static class LoginUserCommandValidator
{
    public static void Validate(this LoginUserCommand request)
    {
        var errors = new List<string>();

        if (!request.Email.Contains("@") || !request.Email.Contains("."))
            errors.Add("Email is not valid.");
        
        if (request.Password.Length < 8)
            errors.Add("Password must be at least 8 characters long.");
        
        if (request.Password.Length > 100)
            errors.Add("Password cannot exceed 100 characters.");
        
        if (!request.Password.Any(char.IsUpper) ||
            !request.Password.Any(char.IsLower) ||
            !request.Password.Any(char.IsDigit))
            errors.Add("Password must contain at least one uppercase letter, one lowercase letter and one digit");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
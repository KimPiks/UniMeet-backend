using UniMeet.Shared.Exceptions;

namespace UniMeet.UserModule.Application.Users.RegisterUser;

public static class RegisterUserValidator
{
    public static void Validate(this RegisterUserCommand request)
    {
        var errors = new List<string>();

        if (request.FirstName.Length < 3) 
            errors.Add("First name must be at least 3 characters long.");
        
        if (request.LastName.Length < 3) 
            errors.Add("Last name must be at least 3 characters long.");
        
        if (request.FirstName.Length > 100)
            errors.Add("First name cannot exceed 100 characters.");
        
        if (request.LastName.Length > 100)
            errors.Add("Last name cannot exceed 100 characters.");
        
        // TODO: better validation
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
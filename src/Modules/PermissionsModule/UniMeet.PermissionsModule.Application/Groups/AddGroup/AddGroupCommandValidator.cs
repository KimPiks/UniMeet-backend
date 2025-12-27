using UniMeet.Shared.Exceptions;

namespace PermissionsModule.Application.Groups.AddGroup;

public static class AddGroupCommandValidator
{
    public static void Validate(this AddGroupCommand request)
    {
        var errors = new List<string>();
        
        if (request.Name.Length < 3)
            errors.Add("Name must be at least 3 characters long.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
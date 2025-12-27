using UniMeet.Shared.Exceptions;

namespace PermissionsModule.Application.Groups.UpdateGroup;

public static class UpdateGroupCommandValidator
{
    public static void Validate(this UpdateGroupCommand request)
    {
        var errors = new List<string>();

        if (request.Id <= 0)
            errors.Add("Id must be greater than zero.");
        
        if (string.IsNullOrEmpty(request.NewName))
            errors.Add("Name cannot be empty.");
        
        if (request.NewName.Length < 3)
            errors.Add("Name must be at least 3 characters long.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
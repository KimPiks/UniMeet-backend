using UniMeet.Shared.Exceptions;

namespace PermissionsModule.Application.Permissions.AddPermission;

public static class AddPermissionCommandValidator
{
    public static void Validate(this AddPermissionCommand request)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(request.PermissionName))
            errors.Add("Permission name cannot be empty.");
        
        if (request.PermissionName.Length < 3)
            errors.Add("Permission name must be at least 3 characters long.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
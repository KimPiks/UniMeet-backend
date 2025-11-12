using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Departments.AddDepartment;

public static class AddDepartmentValidator
{
    public static void Validate(this AddDepartmentCommand request)
    {
        var errors = new List<string>();

        if (request.UniversityId <= 0)
            errors.Add("UniversityId must be greater than zero.");
        
        if (string.IsNullOrWhiteSpace(request.DepartmentName))
            errors.Add("Name is required.");
        
        if (request.DepartmentName.Length > 100)
            errors.Add("Name must not exceed 100 characters.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
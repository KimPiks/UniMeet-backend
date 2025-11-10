using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Departments.DeleteDepartment;

public static class DeleteDepartmentValidator
{
    public static void Validate(this DeleteDepartmentCommand request)
    {
        var errors = new List<string>();
        
        if (request.DepartmentId <= 0)
            errors.Add("Invalid DepartmentId. It must be a positive integer.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
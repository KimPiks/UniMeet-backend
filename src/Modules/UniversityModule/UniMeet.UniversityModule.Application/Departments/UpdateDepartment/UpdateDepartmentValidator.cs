using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Departments.UpdateDepartment;

public static class UpdateDepartmentValidator
{
    public static void Validate(this UpdateDepartmentCommand request)
    {
        var errors = new List<string>();

        if (request.DepartmentId <= 0)
            errors.Add("Invalid DepartmentId.");

        if (string.IsNullOrEmpty(request.NewDepartmentName)) 
            errors.Add("New Department Name cannot be empty.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
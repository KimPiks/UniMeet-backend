using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Departments.GetDepartmentById;

public static class GetDepartmentByIdValidator
{
    public static void Validate(this GetDepartmentByIdQuery request)
    {
        var errors = new List<string>();

        if (request.DepartmentId <= 0)
            errors.Add("DepartmentId must be greater than zero.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
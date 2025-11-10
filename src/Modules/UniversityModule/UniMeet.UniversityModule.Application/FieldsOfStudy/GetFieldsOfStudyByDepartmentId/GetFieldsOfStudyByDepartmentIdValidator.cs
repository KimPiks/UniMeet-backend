using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;

public static class GetFieldsOfStudyByDepartmentIdValidator
{
    public static void Validate(this GetFieldsOfStudyByDepartmentIdQuery request)
    {
        var errors = new List<string>();
        
        if (request.DepartmentId <= 0)
            errors.Add("DepartmentId must be a positive integer.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
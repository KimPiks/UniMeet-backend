using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;

public static class AddFieldOfStudyValidator
{
    public static void Validate(this AddFieldOfStudyCommand request)
    {
        var errors = new List<string>();

        if (request.DepartmentId <= 0)
            errors.Add("DepartmentId must be greater than zero.");
        
        if (string.IsNullOrWhiteSpace(request.FieldOfStudyName))
            errors.Add("Name is required.");
        
        if (request.FieldOfStudyName.Length > 100)
            errors.Add("Name cannot exceed 100 characters.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
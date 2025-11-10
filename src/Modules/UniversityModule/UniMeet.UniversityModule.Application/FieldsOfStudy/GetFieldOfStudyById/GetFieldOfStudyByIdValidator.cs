using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;

public static class GetFieldOfStudyByIdValidator
{
    public static void Validate(this GetFieldOfStudyByIdQuery request)
    {
        var errors = new List<string>();
        
        if (request.FieldOfStudyId <= 0)
            errors.Add("Field of Study ID must be a positive integer.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
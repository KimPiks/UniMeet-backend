using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.UpdateFieldOfStudy;

public static class UpdateFieldOfStudyValidator
{
    public static void Validate(this UpdateFieldOfStudyCommand request)
    {
        var errors = new List<string>();

        if (request.FieldOfStudyId <= 0)
            errors.Add("Field of study ID must be a positive integer.");
        
        if (string.IsNullOrWhiteSpace(request.NewFieldOfStudyName))
            errors.Add("Field of study name cannot be empty.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
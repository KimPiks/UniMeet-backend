using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;

public static class DeleteFieldOfStudyValidator
{
    public static void Validate(this DeleteFieldOfStudyCommand request)
    {
        var errors = new List<string>();

        if (request.FieldOfStudyId <= 0) 
            errors.Add("FieldOfStudyId must be greater than zero.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
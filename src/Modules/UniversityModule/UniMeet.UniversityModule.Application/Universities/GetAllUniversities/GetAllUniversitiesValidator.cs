using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Universities.GetAllUniversities;

public static class GetAllUniversitiesValidator
{
    public static void Validate(this GetAllUniversitiesQuery request)
    {
        var errors = new List<string>();
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
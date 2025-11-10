using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Universities.GetUniversityById;

public static class GetUniversityByIdValidator
{
    public static void Validate(this GetUniversityByIdQuery request)
    {
        var errors = new List<string>();

        if (request.UniversityId <= 0)
            errors.Add("Invalid University Id.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
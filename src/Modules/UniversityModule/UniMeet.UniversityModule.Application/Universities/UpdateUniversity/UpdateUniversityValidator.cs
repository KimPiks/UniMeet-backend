using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Universities.UpdateUniversity;

public static class UpdateUniversityValidator
{
    public static void Validate(this UpdateUniversityCommand request)
    {
        var errors = new List<string>();

        if (request.UniversityId <= 0)
            errors.Add("UniversityId is required.");
        
        if (string.IsNullOrEmpty(request.Name))
            errors.Add("University name is required.");
        
        if (string.IsNullOrEmpty(request.Country))
            errors.Add("Country is required.");
        
        if (string.IsNullOrEmpty(request.Voivodeship))
            errors.Add("Voivodeship is required.");

        if (string.IsNullOrEmpty(request.City))
            errors.Add("City is required.");
        
        if (string.IsNullOrEmpty(request.Address))
            errors.Add("Address is required.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
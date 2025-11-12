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
        
        if (request.Name.Length > 100)
            errors.Add("University name cannot exceed 100 characters.");
        
        if (request.Address.Length > 100)
            errors.Add("University address cannot exceed 100 characters.");
        
        if (request.Country.Length > 100)
            errors.Add("University country cannot exceed 100 characters.");
        
        if (request.City.Length > 100)
            errors.Add("University city cannot exceed 100 characters.");
        
        if (request.Voivodeship.Length > 100)
            errors.Add("University voivodeship cannot exceed 100 characters.");
        
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
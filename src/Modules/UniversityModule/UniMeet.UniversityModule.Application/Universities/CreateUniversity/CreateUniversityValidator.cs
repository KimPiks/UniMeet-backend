using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Universities.CreateUniversity;

public static class CreateUniversityValidator
{
    public static void Validate(this CreateUniversityCommand request)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("University name is required.");
        
        if (string.IsNullOrWhiteSpace(request.Address))
            errors.Add("University address is required.");
        
        if (string.IsNullOrWhiteSpace(request.Country))
            errors.Add("University country is required.");
        
        if (string.IsNullOrWhiteSpace(request.City))
            errors.Add("University city is required.");
        
        if (string.IsNullOrWhiteSpace(request.Voivodeship))
            errors.Add("University voivodeship is required.");
        
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
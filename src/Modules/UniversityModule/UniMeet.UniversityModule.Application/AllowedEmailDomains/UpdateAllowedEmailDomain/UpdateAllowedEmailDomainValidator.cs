using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.UpdateAllowedEmailDomain;

public static class UpdateAllowedEmailDomainValidator
{
    public static void Validate(this UpdateAllowedEmailDomainCommand request)
    {
        var errors = new List<string>();

        if (request.DomainId <= 0)
            errors.Add("Invalid Allowed Email Domain Id.");
        
        if (string.IsNullOrWhiteSpace(request.NewDomain))
            errors.Add("New domain cannot be empty.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
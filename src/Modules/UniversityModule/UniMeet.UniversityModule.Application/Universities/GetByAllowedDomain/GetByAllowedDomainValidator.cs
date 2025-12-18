using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.Universities.GetByAllowedDomain;

public static class GetByAllowedDomainValidator
{
    public static void Validate(this GetByAllowedDomainQuery request)
    {
        var errors = new List<string>();
        
        if (!request.Domain.Contains("."))
            errors.Add("Invalid email domain.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
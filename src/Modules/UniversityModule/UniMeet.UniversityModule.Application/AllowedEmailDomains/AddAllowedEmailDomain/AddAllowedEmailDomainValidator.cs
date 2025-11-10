using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.AddAllowedEmailDomain;

public static class AddAllowedEmailDomainValidator
{
    public static void Validate(this AddAllowedEmailDomainCommand request)
    {
        var errors = new List<string>();

        if (request.UniversityId <= 0)
            errors.Add("UniversityId must be greater than zero.");

        if (string.IsNullOrWhiteSpace(request.Domain))
            errors.Add("Domain cannot be empty.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
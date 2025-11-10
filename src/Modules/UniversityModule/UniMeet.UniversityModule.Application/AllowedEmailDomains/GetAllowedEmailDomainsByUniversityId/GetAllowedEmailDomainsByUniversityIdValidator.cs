using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;

public static class GetAllowedEmailDomainsByUniversityIdValidator
{
    public static void Validate(this GetAllowedEmailDomainsByUniversityIdQuery request)
    {
        var errors = new List<string>();

        if (request.UniversityId <= 0)
            errors.Add("UniversityId must be greater than zero.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
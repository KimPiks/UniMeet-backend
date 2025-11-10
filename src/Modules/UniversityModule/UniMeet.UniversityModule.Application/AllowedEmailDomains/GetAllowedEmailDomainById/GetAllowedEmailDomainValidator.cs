using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;

public static class GetAllowedEmailDomainByIdValidator
{
    public static void Validate(this GetAllowedEmailDomainByIdQuery request)
    {
        var errors = new List<string>();

        if (request.DomainId <= 0)
            errors.Add("Id must be greater than zero.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
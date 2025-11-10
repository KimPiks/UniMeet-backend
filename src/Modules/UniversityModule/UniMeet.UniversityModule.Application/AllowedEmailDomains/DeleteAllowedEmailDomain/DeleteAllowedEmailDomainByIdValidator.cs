using UniMeet.Shared.Exceptions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.DeleteAllowedEmailDomain;

public static class DeleteAllowedEmailDomainByIdValidator
{
    public static void Validate(this DeleteAllowedEmailDomainCommand request)
    {
        var errors = new List<string>();

        if (request.DomainId <= 0)
            errors.Add("Invalid Allowed Email Domain Id.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
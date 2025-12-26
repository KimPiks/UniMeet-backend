using UniMeet.UserModule.Domain.Services;

namespace UniMeet.UserModule.Application.Services;

public class ConfirmationLinkService(string WebsiteUrl) : IConfirmationLinkService
{
    public string Create(Guid confirmationCode)
    {
        return $"{WebsiteUrl}/User/ConfirmAccount/?code={confirmationCode}";
    }
}
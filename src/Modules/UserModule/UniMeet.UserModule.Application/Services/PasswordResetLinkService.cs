using UniMeet.UserModule.Domain.Services;

namespace UniMeet.UserModule.Application.Services;

public class PasswordResetLinkService(string WebsiteUrl) : IPasswordResetLinkService
{
    public string Create(Guid confirmationCode)
    {
        return $"{WebsiteUrl}/User/PasswordReset/?code={confirmationCode}";
    }
}
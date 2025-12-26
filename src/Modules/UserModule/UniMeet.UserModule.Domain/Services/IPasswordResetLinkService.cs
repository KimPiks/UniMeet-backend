namespace UniMeet.UserModule.Domain.Services;

public interface IPasswordResetLinkService
{
    string Create(Guid passwordResetCode);
}
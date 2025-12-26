namespace UniMeet.UserModule.Domain.Services;

public interface IConfirmationLinkService
{
    string Create(Guid confirmationCode);
}
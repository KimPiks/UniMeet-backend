using MailingModule.Commands;
using ModularSystem.Contracts.Mailing;
using ModularSystem.Contracts.User.UserDetails;
using ModularSystem.Contracts.User.Users.LoginUser;
using ModularSystem.Contracts.User.Users.RegisterUser;
using UniMeet.Shared.Exceptions;
using UniMeet.UserModule.Application.Services;
using UniMeet.UserModule.Application.Users.LoginUser;
using UniMeet.UserModule.Application.Users.RegisterUser;

namespace UniMeet.UnitTests.Modules;

public class ValidatorTests
{
    [Fact]
    public void RegisterUserValidator_accepts_valid_command()
    {
        var command = new RegisterUserCommand("Anna", "Kowalska", "anna@uni.edu", "Password1", Sex.Female);

        command.Validate();
    }

    [Fact]
    public void RegisterUserValidator_collects_multiple_errors()
    {
        var command = new RegisterUserCommand("A", "K", "bad-email", "short", Sex.Female);

        var exception = Assert.Throws<ValidationException>(() => command.Validate());

        Assert.Contains("First name must be at least 3 characters long.", exception.Errors);
        Assert.Contains("Last name must be at least 3 characters long.", exception.Errors);
        Assert.Contains("Email is not valid.", exception.Errors);
        Assert.Contains("Password must be at least 8 characters long.", exception.Errors);
    }

    [Fact]
    public void LoginUserValidator_rejects_password_without_digit()
    {
        var command = new LoginUserCommand("anna@uni.edu", "PasswordWithoutDigit");

        var exception = Assert.Throws<ValidationException>(() => command.Validate());

        Assert.Contains("Password must contain at least one uppercase letter, one lowercase letter and one digit", exception.Errors);
    }

    [Fact]
    public void SendEmailCommandValidator_rejects_invalid_email()
    {
        var command = new SendEmailCommand("bad", EmailType.RegisterConfirmation, []);

        var exception = Assert.Throws<ValidationException>(() => command.Validate());

        Assert.Contains("Recipient email address cannot be empty.", exception.Errors);
    }

    [Fact]
    public void ProfilePictureValidator_rejects_unsupported_mime_type_before_loading_image()
    {
        var validator = new ProfilePictureValidator();

        var result = validator.ValidateProfilePicture([1, 2, 3], "avatar.gif", "image/gif");

        Assert.False(result.IsValid);
        Assert.Equal("Invalid file type. Only JPG and PNG files are allowed. Provided: image/gif", result.ErrorMessage);
    }
}

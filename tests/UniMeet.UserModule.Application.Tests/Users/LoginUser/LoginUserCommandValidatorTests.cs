using FluentAssertions;
using UniMeet.Shared.Exceptions;
using UniMeet.UserModule.Application.Users.LoginUser;

namespace UniMeet.UserModule.Application.Tests.Users.LoginUser;

public class LoginUserCommandValidatorTests
{
    [Fact]
    public void Validate_WithValidData_ShouldNotThrowException()
    {
        // Arrange
        var command = new LoginUserCommand(
            "john.doe@example.com",
            "Password123");

        // Act
        Action act = () => command.Validate();

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("no-at-sign.com")]
    [InlineData("no-dot@com")]
    [InlineData("")]
    public void Validate_WithInvalidEmail_ShouldThrowValidationException(string email)
    {
        // Arrange
        var command = new LoginUserCommand(email, "Password123");

        // Act
        Action act = () => command.Validate();

        // Assert
        act.Should().Throw<ValidationException>()
            .And.Errors.Should().Contain("Email is not valid.");
    }

    [Theory]
    [InlineData("short")]
    [InlineData("Pass12")]
    [InlineData("")]
    public void Validate_WithTooShortPassword_ShouldThrowValidationException(string password)
    {
        // Arrange
        var command = new LoginUserCommand("john.doe@example.com", password);

        // Act
        Action act = () => command.Validate();

        // Assert
        act.Should().Throw<ValidationException>()
            .And.Errors.Should().Contain("Password must be at least 8 characters long.");
    }

    [Fact]
    public void Validate_WithTooLongPassword_ShouldThrowValidationException()
    {
        // Arrange
        var command = new LoginUserCommand(
            "john.doe@example.com",
            new string('A', 101));

        // Act
        Action act = () => command.Validate();

        // Assert
        act.Should().Throw<ValidationException>()
            .And.Errors.Should().Contain("Password cannot exceed 100 characters.");
    }

    [Theory]
    [InlineData("password123")] // no uppercase
    [InlineData("PASSWORD123")] // no lowercase
    [InlineData("PasswordABC")] // no digit
    [InlineData("alllowercase123")] // no uppercase
    public void Validate_WithPasswordMissingRequiredCharacters_ShouldThrowValidationException(string password)
    {
        // Arrange
        var command = new LoginUserCommand("john.doe@example.com", password);

        // Act
        Action act = () => command.Validate();

        // Assert
        act.Should().Throw<ValidationException>()
            .And.Errors.Should().Contain("Password must contain at least one uppercase letter, one lowercase letter and one digit");
    }

    [Fact]
    public void Validate_WithMultipleErrors_ShouldThrowValidationExceptionWithAllErrors()
    {
        // Arrange
        var command = new LoginUserCommand(
            "invalid", // invalid email
            "short"); // too short and missing requirements

        // Act
        Action act = () => command.Validate();

        // Assert
        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().HaveCountGreaterThan(1);
    }
}

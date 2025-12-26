using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Application.PasswordResetCodes.CheckIfResetPasswordCodeExists;
using UniMeet.UserModule.Application.PasswordResetCodes.RequestPasswordReset;
using UniMeet.UserModule.Application.PasswordResetCodes.ResetPassword;
using UniMeet.UserModule.Application.RefreshTokens.RefreshTokens;
using UniMeet.UserModule.Application.Users.ConfirmAccount;
using UniMeet.UserModule.Application.Users.LoginUser;
using UniMeet.UserModule.Application.Users.Logout;
using UniMeet.UserModule.Application.Users.RegisterUser;
using UniMeet.UserModule.Domain.Models;

namespace UniMeet.API.Controllers.User;

[ApiController]
[Route("[controller]/[action]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [OnlyAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);
        
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "User registered successfully"));
    }

    [HttpPost]
    [OnlyAnonymous]
    public async Task<IActionResult> ConfirmAccount([FromQuery] Guid code)
    {
        var command = new ConfirmAccountCommand(code);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Account confirmed successfully"));
    }

    [HttpGet]
    [OnlyAnonymous]
    public async Task<IActionResult> CheckIfPasswordResetCodeExists([FromQuery] Guid code)
    {
        var exists = await mediator.SendAsync(new CheckIfResetPasswordCodeExistsQuery(code));
        if (exists) return Ok(ApiResponse<bool>.Ok(true, "Password reset code exists"));
        return Ok(ApiResponse<bool>.Ok(false, "Password reset code does not exist"));
    }

    [HttpPost]
    [OnlyAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest request)
    {
        var passwordReset = new ResetPasswordCommand(request.Code, request.NewPassword);
        await mediator.SendAsync(passwordReset);
        return Ok(ApiResponse<string>.Ok(null, "Password reset successfully"));
    }

    [HttpPost]
    [OnlyAnonymous]
    public async Task<IActionResult> RequestPasswordReset([FromBody] string email)
    {
        var command = new RequestPasswordResetCommand(email);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Password reset requested successfully"));
    }

    [HttpPost]
    [OnlyAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var tokens = await mediator.SendAsync(command);
        return Ok(ApiResponse<LoginTokens>.Ok(tokens, "User logged in successfully"));
    }

    [HttpPost]
    [ActiveUser]
    public async Task<IActionResult> RefreshTokens([FromBody] string refreshToken)
    {
        var command = new RefreshTokensCommand(refreshToken);
        var tokens = await mediator.SendAsync(command);
        return Ok(ApiResponse<LoginTokens>.Ok(tokens, "Tokens refreshed successfully"));
    }

    [HttpPost]
    [ActiveUser]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        var command = new LogoutCommand(refreshToken);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "User logged out successfully"));
    }
}
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.FieldsOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;
using UniMeet.UserEnrollmentModule.Application.UserAffiliations.AddAffiliation;
using UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetAffiliationByUserId;
using UniMeet.UserModule.Application.Interests;
using UniMeet.UserModule.Application.Interests.CreateInterest;
using UniMeet.UserModule.Application.Interests.DeleteInterest;
using UniMeet.UserModule.Application.Interests.GetAllInterests;
using UniMeet.UserModule.Application.Interests.GetInterestById;
using UniMeet.UserModule.Application.PasswordResetCodes.CheckIfResetPasswordCodeExists;
using UniMeet.UserModule.Application.PasswordResetCodes.RequestPasswordReset;
using UniMeet.UserModule.Application.PasswordResetCodes.ResetPassword;
using UniMeet.UserModule.Application.RefreshTokens.RefreshTokens;
using UniMeet.UserModule.Application.Users;
using UniMeet.UserModule.Application.Users.ConfirmAccount;
using UniMeet.UserModule.Application.Users.GetAllUsers;
using UniMeet.UserModule.Application.Users.LoginUser;
using UniMeet.UserModule.Application.Users.Logout;
using UniMeet.UserModule.Application.Users.RegisterUser;
using UniMeet.UserModule.Application.Users.SetGroup;
using UniMeet.UserModule.Application.UserDetails;
using UniMeet.UserModule.Application.UserDetails.GetUserDetailById;
using UniMeet.UserModule.Application.UserDetails.GetUserDetailByUserId;
using UniMeet.UserModule.Application.UserDetails.UpdateUserDetail;
using UniMeet.UserModule.Application.UserDetails.UploadProfilePicture;
using UniMeet.UserModule.Application.UserDetails.DeleteProfilePicture;
using UniMeet.UserModule.Application.UserDetails.GetProfilePicture;
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
            request.Password,
            request.Sex);
        
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
    public async Task<IActionResult> RequestPasswordReset([FromBody] EmailRequest request)
    {
        var command = new RequestPasswordResetCommand(request.Email);
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
    [Permission("UserModule.RefreshTokens")]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokensCommand(request.RefreshToken);
        var tokens = await mediator.SendAsync(command);
        return Ok(ApiResponse<LoginTokens>.Ok(tokens, "Tokens refreshed successfully"));
    }

    [HttpPost]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.Logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        var command = new LogoutCommand(request.RefreshToken);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "User logged out successfully"));
    }

    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.GetUsers")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetAllUsersQuery(offset, limit);
        var users = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users, "Users retrieved successfully"));
    }

    [HttpPatch]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.SetGroup")]
    public async Task<IActionResult> SetGroup([FromBody] SetGroupRequest request)
    {
        var command = new SetGroupCommand(request.UserId, request.GroupId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "User group set successfully")); 
    }

    [HttpPost]
    [Authorize]
    [ActiveUser]
    [Permission("UserEnrollmentModule.EnrollInCourse")]
    public async Task<IActionResult> EnrollInCourse([FromBody] EnrollInCourseRequest request)
    {
        var fieldOfStudyQuery = new GetFieldOfStudyByIdQuery(request.FieldOfStudyId);
        var fieldOfStudy = await mediator.SendAsync(fieldOfStudyQuery);
        if (fieldOfStudy == null) {
            return Ok(ApiResponse<string>.Ok(null, "Field of study not found"));
        }
        
        var command = new AddAffiliationCommand(request.UserId, request.FieldOfStudyId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "User enrolled in course successfully"));
    }

    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserEnrollmentModule.GetUserAffiliations")]
    public async Task<IActionResult> GetUserAffiliations([FromQuery] Guid userId)
    {
        var query = new GetAffiliationByUserIdQuery(userId);
        var affiliation = await mediator.SendAsync(query);
        if (affiliation == null)
        {
            return Ok(ApiResponse<string>.Ok(null, "User has no affiliations"));
        }

        var fieldOfStudyQuery = new GetFieldOfStudyByIdQuery(affiliation.FieldOfStudyId);
        var fieldOfStudy = await mediator.SendAsync(fieldOfStudyQuery);
        if (fieldOfStudy == null)
        {
            return Ok(ApiResponse<string>.Ok(null, "Field of study not found"));
        }

        return Ok(ApiResponse<FieldOfStudyDto>.Ok(fieldOfStudy, "User affiliations retrieved successfully"));
    }
    
    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.GetUserDetail")]
    public async Task<IActionResult> GetUserDetailById([FromQuery] int userDetailId)
    {
        var query = new GetUserDetailByIdQuery(userDetailId);
        var userDetail = await mediator.SendAsync(query);
        if (userDetail == null)
        {
            return Ok(ApiResponse<UserDetailDto>.Ok(null, "User detail not found"));
        }
        return Ok(ApiResponse<UserDetailDto>.Ok(userDetail, "User detail retrieved successfully"));
    }

    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.GetUserDetail")]
    public async Task<IActionResult> GetUserDetailByUserId([FromQuery] Guid userId)
    {
        var query = new GetUserDetailByUserIdQuery(userId);
        var userDetail = await mediator.SendAsync(query);
        if (userDetail == null)
        {
            return Ok(ApiResponse<UserDetailDto>.Ok(null, "User detail not found"));
        }
        return Ok(ApiResponse<UserDetailDto>.Ok(userDetail, "User detail retrieved successfully"));
    }

    [HttpPut]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.UpdateUserDetail")]
    public async Task<IActionResult> UpdateUserDetail([FromBody] UpdateUserDetailRequest request)
    {
        var command = new UpdateUserDetailCommand(request.UserDetailId, request.InterestIds);
        var userDetail = await mediator.SendAsync(command);
        return Ok(ApiResponse<UserDetailDto>.Ok(userDetail, "User detail updated successfully"));
    }

    
    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.GetInterests")]
    public async Task<IActionResult> GetAllInterests([FromQuery] int offset = 0, [FromQuery] int limit = 100)
    {
        var query = new GetAllInterestsQuery(offset, limit);
        var interests = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<InterestDto>>.Ok(interests, "Interests retrieved successfully"));
    }

    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.GetInterests")]
    public async Task<IActionResult> GetInterestById([FromQuery] int interestId)
    {
        var query = new GetInterestByIdQuery(interestId);
        var interest = await mediator.SendAsync(query);
        if (interest == null)
        {
            return Ok(ApiResponse<InterestDto>.Ok(null, "Interest not found"));
        }
        return Ok(ApiResponse<InterestDto>.Ok(interest, "Interest retrieved successfully"));
    }

    [HttpPost]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.CreateInterest")]
    public async Task<IActionResult> CreateInterest([FromBody] CreateInterestRequest request)
    {
        var command = new CreateInterestCommand(request.Name);
        var interest = await mediator.SendAsync(command);
        return Ok(ApiResponse<InterestDto>.Ok(interest, "Interest created successfully"));
    }

    [HttpDelete]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.DeleteInterest")]
    public async Task<IActionResult> DeleteInterest([FromQuery] int interestId)
    {
        var command = new DeleteInterestCommand(interestId);
        await mediator.SendAsync(command);
        return Ok(ApiResponse<string>.Ok(null, "Interest deleted successfully"));
    }

    [HttpPost]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.UpdateUserDetail")]
    public async Task<IActionResult> UploadProfilePicture([FromQuery] int userDetailId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Fail("No file provided"));
        }

        // Get UserId from JWT token
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized(ApiResponse<string>.Fail("Invalid user token"));
        }

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var fileContent = memoryStream.ToArray();
            var mimeType = file.ContentType ?? "application/octet-stream";

            var command = new UploadProfilePictureCommand(userDetailId, userId, fileContent, file.FileName, mimeType);
            var userDetail = await mediator.SendAsync(command);
            return Ok(ApiResponse<UserDetailDto>.Ok(userDetail, "Profile picture uploaded successfully"));
        }
    }

    [HttpDelete]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.UpdateUserDetail")]
    public async Task<IActionResult> DeleteProfilePicture([FromQuery] int userDetailId)
    {
        var command = new DeleteProfilePictureCommand(userDetailId);
        var userDetail = await mediator.SendAsync(command);
        return Ok(ApiResponse<UserDetailDto>.Ok(userDetail, "Profile picture deleted successfully"));
    }

    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.GetUserDetail")]
    public async Task<IActionResult> GetProfilePicture([FromQuery] int userDetailId)
    {
        var query = new GetProfilePictureQuery(userDetailId);
        var picture = await mediator.SendAsync(query);
        
        return File(picture.PictureData, picture.MimeType, picture.FileName);
    }
}



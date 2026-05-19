using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Attributes;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using ModularSystem;
using ModularSystem.Contracts.University.FieldsOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldOfStudyById;
using ModularSystem.Contracts.UserEnrollment.UserAffiliations.AddAffiliation;
using ModularSystem.Contracts.UserEnrollment.UserAffiliations.GetAffiliationByUserId;
using ModularSystem.Contracts.User.Interests;
using ModularSystem.Contracts.User.Interests.CreateInterest;
using ModularSystem.Contracts.User.Interests.DeleteInterest;
using ModularSystem.Contracts.User.Interests.GetAllInterests;
using ModularSystem.Contracts.User.Interests.GetInterestById;
using ModularSystem.Contracts.User.PasswordResetCodes.CheckIfResetPasswordCodeExists;
using ModularSystem.Contracts.User.PasswordResetCodes.RequestPasswordReset;
using ModularSystem.Contracts.User.PasswordResetCodes.ResetPassword;
using ModularSystem.Contracts.User.RefreshTokens.RefreshTokens;
using ModularSystem.Contracts.User.Users;
using ModularSystem.Contracts.User.Users.ConfirmAccount;
using ModularSystem.Contracts.User.Users.GetAllUsers;
using ModularSystem.Contracts.User.Users.GetUserById;
using ModularSystem.Contracts.User.Users.LoginUser;
using ModularSystem.Contracts.User.Users.Logout;
using ModularSystem.Contracts.User.Users.RegisterUser;
using ModularSystem.Contracts.User.Users.SetGroup;
using ModularSystem.Contracts.User.UserDetails;
using ModularSystem.Contracts.User.UserDetails.GetUserDetailById;
using ModularSystem.Contracts.User.UserDetails.GetUserDetailByUserId;
using ModularSystem.Contracts.User.UserDetails.UpdateUserDetail;
using ModularSystem.Contracts.User.UserDetails.UploadProfilePicture;
using ModularSystem.Contracts.User.UserDetails.DeleteProfilePicture;
using ModularSystem.Contracts.User.UserDetails.GetProfilePicture;
using ModularSystem.Contracts.User.Users.SearchUsers;
using ModularSystem.Contracts.User.Models;

namespace UniMeet.API.Controllers.User;

[ApiController]
[Route("[controller]/[action]")]
public class UserController(IModuleRequestDispatcher mediator) : ControllerBase
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

    [HttpGet]
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
    [AllowAnonymous]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokensCommand(request.RefreshToken);
        var tokens = await mediator.SendAsync(command);
        return Ok(ApiResponse<LoginTokens>.Ok(tokens, "Tokens refreshed successfully"));
    }

    [HttpPost]
    [AllowAnonymous]
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

    [HttpGet]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.GetUsers")]
    public async Task<IActionResult> GetUserById([FromQuery] Guid userId)
    {
        var query = new GetUserByIdQuery(userId);
        var user = await mediator.SendAsync(query);
        if (user == null)
        {
            return Ok(ApiResponse<UserDto>.Ok(null, "User not found"));
        }
        return Ok(ApiResponse<UserDto>.Ok(user, "User retrieved successfully"));
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
        
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var enrollUserId))
            return Unauthorized(ApiResponse<string>.Fail("Invalid user token"));

        var command = new AddAffiliationCommand(enrollUserId, request.FieldOfStudyId);
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
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(ApiResponse<string>.Fail("Invalid user token"));

        var command = new UpdateUserDetailCommand(request.UserDetailId, userId, request.InterestIds);
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
    public async Task<IActionResult> UploadProfilePicture([FromQuery] int userDetailId, IFormFile? file)
        => await UploadOrUpdateProfilePictureAsync(userDetailId, file, "Profile picture uploaded successfully");

    [HttpPost]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.UpdateUserDetail")]
    public async Task<IActionResult> UpdateProfilePicture([FromQuery] int userDetailId, IFormFile? file)
        => await UploadOrUpdateProfilePictureAsync(userDetailId, file, "Profile picture updated successfully");

    [HttpDelete]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.UpdateUserDetail")]
    public async Task<IActionResult> DeleteProfilePicture([FromQuery] int userDetailId)
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized(ApiResponse<string>.Fail("Invalid user token"));

        var command = new DeleteProfilePictureCommand(userDetailId, userId);
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

    [HttpPost]
    [Authorize]
    [ActiveUser]
    [Permission("UserModule.SearchUsers")]
    public async Task<IActionResult> SearchUsers([FromBody] SearchUsersRequest request)
    {
        var profile = new UserSearchProfile(
            request.Profile.UniversityId,
            request.Profile.FieldOfStudyId,
            request.Profile.InterestIds,
            request.Profile.Sex);

        UserSearchFilters? filters = null;
        if (request.Filters != null)
        {
            filters = new UserSearchFilters(
                request.Filters.UniversityId,
                request.Filters.FieldOfStudyId,
                request.Filters.InterestIds,
                request.Filters.Sex);
        }

        var query = new SearchUsersQuery(profile, filters, request.Offset, request.Padding);
        var users = await mediator.SendAsync(query);
        return Ok(ApiResponse<IEnumerable<SearchUserDto>>.Ok(users, "Users retrieved successfully"));
    }

    private async Task<IActionResult> UploadOrUpdateProfilePictureAsync(int userDetailId, IFormFile? file, string successMessage)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponse<string>.Fail("No file provided"));
        }

        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized(ApiResponse<string>.Fail("Invalid user token"));
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var fileContent = memoryStream.ToArray();
        var mimeType = file.ContentType;
        var command = new UploadProfilePictureCommand(userDetailId, userId, fileContent, file.FileName, mimeType);
        var userDetail = await mediator.SendAsync(command);

        return Ok(ApiResponse<UserDetailDto>.Ok(userDetail, successMessage));
    }
}



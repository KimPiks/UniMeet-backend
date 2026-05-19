using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModularSystem.Contracts.University.FieldsOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldOfStudyById;
using ModularSystem.Contracts.User.Models;
using ModularSystem.Contracts.User.UserDetails;
using ModularSystem.Contracts.User.UserDetails.UploadProfilePicture;
using ModularSystem.Contracts.User.Users.LoginUser;
using ModularSystem.Contracts.User.Users.RegisterUser;
using ModularSystem.Contracts.UserEnrollment.UserAffiliations.AddAffiliation;
using UniMeet.API.Controllers.User;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using ContractSex = ModularSystem.Contracts.User.UserDetails.Sex;

namespace UniMeet.UnitTests.Api;

public class UserControllerTests
{
    [Fact]
    public async Task Register_Sends_register_command_and_returns_success_response()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var controller = new UserController(dispatcher);
        var request = new RegisterUserRequest("Anna", "Kowalska", "anna@uni.edu", "Password1", ContractSex.Female);

        var result = await controller.Register(request);

        ControllerTestHelpers.AssertOkResponse<string>(result, "User registered successfully");
        var command = Assert.IsType<RegisterUserCommand>(dispatcher.SentRequests.Single());
        Assert.Equal(request.FirstName, command.FirstName);
        Assert.Equal(request.LastName, command.LastName);
        Assert.Equal(request.Email, command.Email);
        Assert.Equal(request.Password, command.Password);
        Assert.Equal(request.Sex, command.Sex);
    }

    [Fact]
    public async Task Login_Returns_tokens_from_module()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var tokens = new LoginTokens("access-token", "refresh-token");
        dispatcher.QueueResult(tokens);
        var controller = new UserController(dispatcher);

        var result = await controller.Login(new LoginRequest
        {
            Email = "student@uni.edu",
            Password = "Password1"
        });

        var response = ControllerTestHelpers.AssertOkResponse<LoginTokens>(result, "User logged in successfully");
        Assert.Same(tokens, response.Data);
        var command = Assert.IsType<LoginUserCommand>(dispatcher.SentRequests.Single());
        Assert.Equal("student@uni.edu", command.Email);
        Assert.Equal("Password1", command.Password);
    }

    [Fact]
    public async Task EnrollInCourse_When_user_claim_is_missing_returns_unauthorized_after_field_lookup()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        dispatcher.QueueResult(new FieldOfStudyDto { Id = 12, Name = "Computer Science" });
        var controller = new UserController(dispatcher);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = await controller.EnrollInCourse(new EnrollInCourseRequest { FieldOfStudyId = 12 });

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        var response = Assert.IsType<ApiResponse<string>>(unauthorized.Value);
        Assert.False(response.Success);
        Assert.Equal("Invalid user token", response.Message);
        Assert.IsType<GetFieldOfStudyByIdQuery>(dispatcher.SentRequests.Single());
    }

    [Fact]
    public async Task EnrollInCourse_With_valid_user_sends_affiliation_command()
    {
        var userId = Guid.NewGuid();
        var dispatcher = new FakeModuleRequestDispatcher();
        dispatcher.QueueResult(new FieldOfStudyDto { Id = 20, Name = "Mathematics" });
        var controller = new UserController(dispatcher);
        controller.SetCurrentUser(userId);

        var result = await controller.EnrollInCourse(new EnrollInCourseRequest { FieldOfStudyId = 20 });

        ControllerTestHelpers.AssertOkResponse<string>(result, "User enrolled in course successfully");
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.IsType<GetFieldOfStudyByIdQuery>(request),
            request =>
            {
                var command = Assert.IsType<AddAffiliationCommand>(request);
                Assert.Equal(userId, command.UserId);
                Assert.Equal(20, command.FieldOfStudyId);
            });
    }

    [Fact]
    public async Task UploadProfilePicture_With_file_sends_profile_picture_command()
    {
        var userId = Guid.NewGuid();
        var dispatcher = new FakeModuleRequestDispatcher();
        var updatedDetail = new UserDetailDto
        {
            Id = 7,
            UserId = userId,
            Sex = ContractSex.Male.ToString(),
            Interests = [],
            ProfilePicturePath = "avatar.png",
            ProfilePictureMimeType = "image/png"
        };
        dispatcher.QueueResult(updatedDetail);
        var controller = new UserController(dispatcher);
        controller.SetCurrentUser(userId);
        var fileBytes = new byte[] { 1, 2, 3, 4 };
        await using var stream = new MemoryStream(fileBytes);
        var file = new FormFile(stream, 0, fileBytes.Length, "file", "avatar.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        var result = await controller.UploadProfilePicture(7, file);

        var response = ControllerTestHelpers.AssertOkResponse<UserDetailDto>(result, "Profile picture uploaded successfully");
        Assert.Same(updatedDetail, response.Data);
        var command = Assert.IsType<UploadProfilePictureCommand>(dispatcher.SentRequests.Single());
        Assert.Equal(7, command.UserDetailId);
        Assert.Equal(userId, command.UserId);
        Assert.Equal(fileBytes, command.FileContent);
        Assert.Equal("avatar.png", command.FileName);
        Assert.Equal("image/png", command.MimeType);
    }

    [Fact]
    public async Task UploadProfilePicture_Without_file_returns_bad_request()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var controller = new UserController(dispatcher);

        var result = await controller.UploadProfilePicture(7, null);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ApiResponse<string>>(badRequest.Value);
        Assert.False(response.Success);
        Assert.Equal("No file provided", response.Message);
        Assert.Empty(dispatcher.SentRequests);
    }
}

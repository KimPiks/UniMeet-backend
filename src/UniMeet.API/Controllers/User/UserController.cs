using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Models.Requests;
using UniMeet.API.Responses;
using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Application.Users.RegisterUser;

namespace UniMeet.API.Controllers.User;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost]
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
}
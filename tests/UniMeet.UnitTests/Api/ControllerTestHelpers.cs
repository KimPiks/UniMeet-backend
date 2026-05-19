using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniMeet.API.Responses;

namespace UniMeet.UnitTests.Api;

internal static class ControllerTestHelpers
{
    public static void SetCurrentUser(this ControllerBase controller, Guid userId)
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };
    }

    public static ApiResponse<T> AssertOkResponse<T>(IActionResult result, string expectedMessage)
    {
        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<T>>(ok.Value);

        Assert.True(response.Success);
        Assert.Equal(expectedMessage, response.Message);
        return response;
    }
}

using UniMeet.API.Responses;

namespace UniMeet.UnitTests.Api;

public class ApiResponseTests
{
    [Fact]
    public void Ok_Creates_success_response_with_data_and_message()
    {
        var response = ApiResponse<int>.Ok(42, "done");

        Assert.True(response.Success);
        Assert.Equal(42, response.Data);
        Assert.Equal("done", response.Message);
        Assert.True(response.TimeStampUtc <= DateTime.UtcNow);
    }

    [Fact]
    public void Fail_Creates_failure_response_with_error_payload()
    {
        var errors = new[] { "first", "second" };

        var response = ApiResponse<string[]>.Fail(errors, "Validation failed");

        Assert.False(response.Success);
        Assert.Same(errors, response.Data);
        Assert.Equal("Validation failed", response.Message);
    }
}

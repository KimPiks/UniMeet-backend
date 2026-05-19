using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using UniMeet.API.Middlewares;
using UniMeet.Shared.Exceptions;

namespace UniMeet.UnitTests.Api;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_Maps_validation_exception_to_bad_request_with_errors()
    {
        var (context, json) = await InvokeWithException(new ValidationException(["Name is required"]));

        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.StartsWith("application/json", context.Response.ContentType);
        using var document = JsonDocument.Parse(json);
        Assert.False(document.RootElement.GetProperty("success").GetBoolean());
        Assert.Equal("Validation failed", document.RootElement.GetProperty("message").GetString());
        Assert.Equal("Name is required", document.RootElement.GetProperty("data")[0].GetString());
    }

    [Fact]
    public async Task InvokeAsync_Maps_unexpected_exception_to_internal_server_error()
    {
        var (context, json) = await InvokeWithException(new InvalidOperationException("boom"));

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        using var document = JsonDocument.Parse(json);
        Assert.False(document.RootElement.GetProperty("success").GetBoolean());
        Assert.Equal("Unexpected server error.", document.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task InvokeAsync_Does_not_write_error_when_next_succeeds()
    {
        var middleware = new ExceptionMiddleware(NullLogger<ExceptionMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context, _ =>
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        });

        Assert.Equal(StatusCodes.Status204NoContent, context.Response.StatusCode);
        Assert.Equal(0, context.Response.Body.Length);
    }

    private static async Task<(DefaultHttpContext Context, string Json)> InvokeWithException(Exception exception)
    {
        var middleware = new ExceptionMiddleware(NullLogger<ExceptionMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context, _ => throw exception);

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        return (context, await reader.ReadToEndAsync());
    }
}

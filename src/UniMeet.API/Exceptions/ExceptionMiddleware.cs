using System.Net;
using UniMeet.API.Responses;
using UniMeet.Shared.Exceptions;

namespace UniMeet.API.Exceptions;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            logger.LogWarning(ex, "Domain error occurred: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.BadRequest, ApiResponse<string>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred");
            await WriteResponse(context, HttpStatusCode.InternalServerError, ApiResponse<string>.Fail("Unexpected server error."));
        }
    }

    private static async Task WriteResponse<T>(HttpContext context, HttpStatusCode statusCode, ApiResponse<T> response)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(response);
    }
}
using System.Net;
using UniMeet.API.Responses;
using UniMeet.Shared.Exceptions;

namespace UniMeet.API.Middlewares;

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
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error occurred: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.BadRequest, ApiResponse<List<string>>.Fail(ex.Errors.ToList(), ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Not found error occured: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.NotFound, ApiResponse<string>.Fail(ex.Message));
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
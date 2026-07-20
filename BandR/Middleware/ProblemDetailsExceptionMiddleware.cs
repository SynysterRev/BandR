using BandR.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BandR.Middleware;

public class ProblemDetailsExceptionMiddleware(RequestDelegate next, ILogger<ProblemDetailsExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception) when (exception is IProblemConvertible problemConvertible)
        {
            logger.LogWarning(exception, "Business exception while handling {Method} {Path}",
                context.Request.Method, context.Request.Path);

            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message,
                Instance = context.Request.Path
            };
            problemConvertible.ToProblemDetails(in problemDetails);

            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            await JsonSerializer.SerializeAsync(
                context.Response.Body,
                problemDetails,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}

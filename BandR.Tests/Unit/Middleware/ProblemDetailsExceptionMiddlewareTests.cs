using BandR.Exceptions;
using BandR.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace BandR.Tests.Unit.Middleware;

public class ProblemDetailsExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldWriteProblemDetails_WhenBusinessExceptionIsThrown()
    {
        var musicianId = Guid.NewGuid();
        var middleware = new ProblemDetailsExceptionMiddleware(
            _ => throw new MusicianException.MusicianNotFoundException(musicianId),
            NullLogger<ProblemDetailsExceptionMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        context.Response.ContentType.Should().Be("application/problem+json");

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var body = await reader.ReadToEndAsync();
        body.Should().Contain("\"type\"");
        body.Should().Contain("Musician/Get/NotFound");
        body.Should().Contain(musicianId.ToString());
    }
}

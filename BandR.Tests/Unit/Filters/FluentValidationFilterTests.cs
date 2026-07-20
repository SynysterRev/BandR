using BandR.DTOs.Musicians;
using BandR.Filters;
using BandR.Validators.Musicians;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BandR.Tests.Unit.Filters;

public class FluentValidationFilterTests
{
    [Fact]
    public async Task OnActionExecutionAsync_ShouldReturnBadRequest_WhenDtoIsInvalid()
    {
        var services = new ServiceCollection();
        services.AddScoped<IValidator<CreateMusicianDto>, CreateMusicianDtoValidator>();
        await using var serviceProvider = services.BuildServiceProvider();
        var filter = new FluentValidationFilter(serviceProvider);
        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        var context = new ActionExecutingContext(
            actionContext,
            [],
            new Dictionary<string, object?>
            {
                ["dto"] = new CreateMusicianDto("", "Montpellier", [], [], [], null)
            },
            new object());
        var nextWasCalled = false;

        await filter.OnActionExecutionAsync(context, () =>
        {
            nextWasCalled = true;
            return Task.FromResult(new ActionExecutedContext(actionContext, [], new object()));
        });

        nextWasCalled.Should().BeFalse();
        context.Result.Should().BeOfType<BadRequestObjectResult>();
        var result = (BadRequestObjectResult)context.Result!;
        result.Value.Should().BeOfType<ValidationProblemDetails>();
    }
}

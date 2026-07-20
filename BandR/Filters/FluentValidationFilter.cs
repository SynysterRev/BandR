using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BandR.Filters;

public sealed class FluentValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var errors = new Dictionary<string, string[]>();

        foreach (var argument in context.ActionArguments.Values.Where(value => value is not null))
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(argument!.GetType());
            if (serviceProvider.GetService(validatorType) is not IValidator validator)
                continue;

            var validationResult = await validator.ValidateAsync(
                new ValidationContext<object>(argument),
                context.HttpContext.RequestAborted);

            foreach (var errorGroup in validationResult.Errors.GroupBy(error => error.PropertyName))
            {
                errors[errorGroup.Key] = errorGroup.Select(error => error.ErrorMessage).ToArray();
            }
        }

        if (errors.Count == 0)
        {
            await next();
            return;
        }

        context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
        {
            Type = "Validation/Failed",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.HttpContext.Request.Path
        });
    }
}

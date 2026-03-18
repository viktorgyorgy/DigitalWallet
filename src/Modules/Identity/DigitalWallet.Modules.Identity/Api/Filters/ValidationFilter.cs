using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DigitalWallet.Modules.Identity.Api.Filters;

public class ValidationFilter<T>(IValidator<T>? validator = null) : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // If no validator is registered for this type, just move to the next step
        if (validator is null) return await next(context);

        // Find the argument of type T (e.g., LoginRequest) in the method arguments
        var input = context.Arguments.OfType<T>().FirstOrDefault();

        if (input is not null)
        {
            var result = await validator.ValidateAsync(input, context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                // Returns a standard 400 Validation Problem with the errors mapped properly
                return Results.ValidationProblem(result.ToDictionary());
            }
        }

        return await next(context);
    }
}

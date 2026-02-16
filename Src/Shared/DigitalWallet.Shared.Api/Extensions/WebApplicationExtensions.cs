using DigitalWallet.Shared.Api.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Shared.Api.Extensions;

public static class WebApplicationExtensions
{
    public static IServiceCollection AddWebInfrastructure(
        this IServiceCollection services)
    {
        services.AddExceptionHandler<DomainExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}

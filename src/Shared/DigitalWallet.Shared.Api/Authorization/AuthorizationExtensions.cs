using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Shared.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddSharedAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthPolicies.UserOnly, policy => policy.RequireAuthenticatedUser());

        return services;
    }
}

using DigitalWallet.Read.Storage;
using DigitalWallet.Read.Storage.Interfaces;
using DigitalWallet.Read.Storage.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Read.Api;

public static class ReadApiExtensions
{
    public static IServiceCollection AddReadSide(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDatabase(configuration);

        services.AddScoped<MongoDbInitializer>();
        services.AddScoped<IUserQueries, UserQueries>();

        return services;
    }

    public static IEndpointRouteBuilder MapReadEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users")
                       .WithTags("Users");

        group.MapGet("/{id:guid}", async (Guid id, IUserQueries queries) =>
        {
            var user = await queries.GetByIdAsync(id);

            return user is not null
                ? Results.Ok(user)
                : Results.NotFound();
        });

        return app;
    }
}

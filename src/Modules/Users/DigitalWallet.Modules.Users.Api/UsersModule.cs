using DigitalWallet.Modules.Users.Application.Interfaces;
using DigitalWallet.Modules.Users.Application.Services;
using DigitalWallet.Modules.Users.Infrastructure.Persistence;
using DigitalWallet.Modules.Users.Infrastructure.Persistence.Repositories;
using DigitalWallet.Shared.Api.Interfaces;
using DigitalWallet.Shared.Domain;
using DigitalWallet.Shared.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Modules.Users.Api;

public class UsersModule : IModule
{
    public string Name => "Users";

    public void RegisterModule(IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddModuleDbContext<UsersDbContext>(configuration, Schemas.Users);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUsersOutboxRepository, UsersOutboxRepository>();

        services.AddScoped<UsersService>();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("users")
            .WithTags("Users");
    }

    public void ApplyMigrations(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        dbContext.Database.Migrate();
    }
}

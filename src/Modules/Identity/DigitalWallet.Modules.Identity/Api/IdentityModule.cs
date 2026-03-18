using DigitalWallet.Modules.Identity.Api.Endpoints;
using DigitalWallet.Modules.Identity.Application.Interfaces;
using DigitalWallet.Modules.Identity.Application.Services;
using DigitalWallet.Modules.Identity.Application.Validators;
using DigitalWallet.Modules.Identity.Infrastructure;
using DigitalWallet.Modules.Identity.Infrastructure.Persistence;
using DigitalWallet.Modules.Identity.Infrastructure.Persistence.Repositories;
using DigitalWallet.Shared.Api.Interfaces;
using DigitalWallet.Shared.Domain;
using DigitalWallet.Shared.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Modules.Identity.Api;

public class IdentityModule : IModule
{
    public string Name => "Identity";

    public void ApplyMigrations(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        dbContext.Database.Migrate();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        IdentityEndpoints.MapIdentityEndpoints(endpoints);
    }
    public void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        // Persistence & Repos
        services.AddModuleDbContext<IdentityDbContext>(configuration, Schemas.Identity);
        services.AddScoped<CredentialRepository>();
        services.AddScoped<RefreshTokenRepository>();
        services.AddScoped<IdentityOutboxRepository>();

        // Security Core
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<TokenService>();
        services.AddScoped<IdentityService>();

        // Validation
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
    }
}

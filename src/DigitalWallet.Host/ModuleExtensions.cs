using DigitalWallet.Modules.Identity.Api;
using DigitalWallet.Modules.Users.Api;
using DigitalWallet.Shared.Api.Interfaces;
using MassTransit;

namespace DigitalWallet.Host;

public static class ModuleExtensions
{
    private static readonly IModule[] _modules = [new UsersModule(), new IdentityModule()];

    public static IServiceCollection RegisterModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        foreach (var module in _modules)
        {
            module.RegisterModule(services, configuration);
            Console.WriteLine($"[Module Loader] Services registered for: {module.Name}");
        }
        return services;
    }

    public static IEndpointRouteBuilder MapModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        foreach (var module in _modules)
        {
            module.MapEndpoints(endpoints);
        }

        return endpoints;
    }

    public static void RunModuleMigrations(this IApplicationBuilder app)
    {
        foreach (var module in _modules)
        {
            module.ApplyMigrations(app);
        }
    }

    public static void RegisterModuleConsumers(IRiderRegistrationConfigurator rider, IConfiguration configuration)
    {
        foreach (var module in _modules)
        {
            module.RegisterConsumers(rider);
            Console.WriteLine($"[Module Loader] Kafka Consumers registered for: {module.Name}");
        }
    }

    public static void ConfigureModuleEndpoints(
    IKafkaFactoryConfigurator kafka,
    IRiderRegistrationContext context,
    IConfiguration configuration)
    {
        foreach (var module in _modules)
        {
            module.ConfigureConsumerEndpoints(kafka, context, configuration);
        }
    }
}

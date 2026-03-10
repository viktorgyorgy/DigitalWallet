using DigitalWallet.Modules.Users.Api;
using DigitalWallet.Shared.Api.Interfaces;

namespace DigitalWallet.Host;

public static class ModuleExtensions
{
    public static IServiceCollection RegisterModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        IModule[] modules = [new UsersModule()];

        foreach (IModule module in modules)
        {
            module.RegisterModule(services, configuration);

            services.AddSingleton(module);

            Console.WriteLine($"[Module Loader] Automatically registered: {module.Name}");
        }

        return services;
    }

    public static IEndpointRouteBuilder MapModuleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var modules = endpoints.ServiceProvider.GetServices<IModule>();

        foreach (var module in modules)
        {
            module.MapEndpoints(endpoints);
            Console.WriteLine($"[Module Loader] Endpoints mapped for: {module.Name}");
        }

        return endpoints;
    }

    public static void RunModuleMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var modules = scope.ServiceProvider.GetServices<IModule>();

        foreach (var module in modules)
        {
            Console.WriteLine($"[Migrator] Applying migrations for: {module.Name}...");

            try
            {
                module.ApplyMigrations(app);
                Console.WriteLine($"[Migrator] Success: {module.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Migrator] Error applying migrations for {module.Name}: {ex.Message}");
                throw;
            }
        }
    }
}

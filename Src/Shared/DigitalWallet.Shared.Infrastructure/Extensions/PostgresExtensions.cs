using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Shared.Infrastructure.Extensions;

public static class PostgresExtensions
{
    public static IServiceCollection AddModuleDbContext<TContext>(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string schema) where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString("PostgresConnection");

        services.AddDbContext<TContext>(options =>
            options.UseNpgsql(connectionString, o => 
            {
                o.MigrationsHistoryTable($"__{schema}_migrations_history", schema);
                o.MigrationsAssembly(typeof(TContext).Assembly.FullName);
            }));

        return services;
    }
}
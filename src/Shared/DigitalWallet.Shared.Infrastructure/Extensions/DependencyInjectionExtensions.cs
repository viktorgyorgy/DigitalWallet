using DigitalWallet.Shared.Application.Interfaces;
using DigitalWallet.Shared.Infrastructure.Interfaces;
using DigitalWallet.Shared.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Shared.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

        return services;
    }
}

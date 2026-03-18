using DigitalWallet.Shared.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Shared.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IKafkaConsumer, KafkaConsumer>();

        return services;
    }
}

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Shared.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration,
    Action<IRiderRegistrationConfigurator>? registerConsumers = null,
    Action<IKafkaFactoryConfigurator, IRiderRegistrationContext>? configureEndpoints = null)
    {
        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

            x.AddRider(rider =>
            {
                registerConsumers?.Invoke(rider);

                rider.UsingKafka((context, k) =>
                {
                    k.Host(configuration["KAFKA_BOOTSTRAP_SERVERS"] ?? "redpanda:29092");

                    configureEndpoints?.Invoke(k, context);
                });
            });
        });

        return services;
    }
}

using DigitalWallet.Modules.Users.Contracts.Events;
using DigitalWallet.ReadWorker.Consumers.Users;
using DigitalWallet.Shared.Domain;
using MassTransit;

namespace DigitalWallet.ReadWorker.Extensions;

public static class ReadWorkerExtensions
{
    public static void RegisterProjections(IRiderRegistrationConfigurator rider)
    {
        rider.AddConsumer<UserCreatedConsumer>();
    }

    public static void ConfigureProjectionEndpoints(IKafkaFactoryConfigurator kafka, IRiderRegistrationContext context, IConfiguration configuration)
    {
        var env = configuration["ENV_NAME"] ?? "dev";

        kafka.TopicEndpoint<UserCreatedIntegrationEvent>(
            $"{env}.{Schemas.Users}.events",
            $"{env}.read-worker.{Schemas.Users}-group",
            e =>
            {
                e.ConfigureConsumer<UserCreatedConsumer>(context);
                e.AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
            });
    }
}

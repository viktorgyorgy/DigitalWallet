namespace DigitalWallet.Shared.Infrastructure.Interfaces;

public interface IKafkaConsumer
{
    Task SubscribeAsync(
        string schema,
        string subscriptionName,
        Func<string, string, Task> handler,
        CancellationToken ct);
}

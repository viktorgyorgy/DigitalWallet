using DigitalWallet.Shared.Domain.Events;

namespace DigitalWallet.Shared.Infrastructure.Interfaces;

public interface IKafkaConsumer
{
    Task SubscribeAsync<TEvent>(Func<TEvent, Task> handler, CancellationToken ct)
        where TEvent : IIntegrationEvent;
}

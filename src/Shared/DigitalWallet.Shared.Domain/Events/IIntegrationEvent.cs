namespace DigitalWallet.Shared.Domain.Events;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}

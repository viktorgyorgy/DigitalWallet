using DigitalWallet.Shared.Domain.Events;
using System.Text.Json;

namespace DigitalWallet.Shared.Application;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public string AggregateId { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }

    private OutboxMessage() { }

    public static OutboxMessage FromEvent(IIntegrationEvent @event, Guid aggregateId)
        => FromEvent(@event, aggregateId.ToString());
    public static OutboxMessage FromEvent(IIntegrationEvent @event, string aggregateId) => new()
    {
        Id = @event.EventId,
        AggregateId = aggregateId,
        Type = @event.GetType().Name,
        Payload = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = false
        }),
        CreatedAt = @event.OccurredOn
    };
}

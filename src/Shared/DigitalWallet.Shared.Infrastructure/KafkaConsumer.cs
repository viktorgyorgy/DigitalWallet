using Confluent.Kafka;
using DigitalWallet.Shared.Domain.Events;
using DigitalWallet.Shared.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DigitalWallet.Shared.Infrastructure.Kafka;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly IConfiguration _config;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(IConfiguration config, ILogger<KafkaConsumer> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SubscribeAsync<TEvent>(Func<TEvent, Task> handler, CancellationToken ct)
        where TEvent : IIntegrationEvent
    {
        var config = new ConsumerConfig
        {
            // Use the internal Redpanda address from your docker-compose
            BootstrapServers = _config["KAFKA_BOOTSTRAP_SERVERS"] ?? "redpanda:29092",
            GroupId = $"read-worker-{typeof(TEvent).Name}",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();

        // Debezium topic naming convention: {prefix}.{schema}.{table}
        var env = _config["ENV_NAME"] ?? "dev";
        var topic = $"{env}.users.users.outbox_messages";

        consumer.Subscribe(topic);
        _logger.LogInformation("Subscribed to topic: {Topic} for event {Event}", topic, typeof(TEvent).Name);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(ct);
                if (result?.Message?.Value == null) continue;

                using var jsonDoc = JsonDocument.Parse(result.Message.Value);

                if (!jsonDoc.RootElement.TryGetProperty("payload", out var payload) ||
                    !payload.TryGetProperty("after", out var after) ||
                    after.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                // --- UPDATE 1: Match full class name ---
                var eventType = after.GetProperty("type").GetString();
                var targetType = typeof(TEvent).Name;

                if (eventType != targetType) continue;

                // --- UPDATE 2: Use "payload" instead of "data" ---
                var eventDataJson = after.GetProperty("payload").GetString();
                if (string.IsNullOrEmpty(eventDataJson)) continue;

                var @event = JsonSerializer.Deserialize<TEvent>(eventDataJson);

                if (@event != null)
                {
                    await handler(@event);
                    _logger.LogDebug("Processed event: {Type}", eventType);
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming Kafka message from Debezium outbox");
                await Task.Delay(2000, ct); // Prevent log spamming on connection failure
            }
        }

        consumer.Close();
    }
}

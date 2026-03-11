using Confluent.Kafka;
using DigitalWallet.Shared.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalWallet.Shared.Infrastructure;

public class KafkaConsumer(IConfiguration config, ILogger<KafkaConsumer> logger) : IKafkaConsumer
{
    private readonly IConfiguration _config = config;
    private readonly ILogger<KafkaConsumer> _logger = logger;

    public async Task SubscribeAsync(string schema, string subscriptionName, Func<string, string, Task> handler, CancellationToken ct)
    {
        var env = _config["ENV_NAME"] ?? "dev";

        var topic = $"{env}.{schema}.events";

        var config = new ConsumerConfig
        {
            BootstrapServers = _config["KAFKA_BOOTSTRAP_SERVERS"] ?? "redpanda:29092",
            GroupId = $"{env}.{schema}.{subscriptionName}-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            AllowAutoCreateTopics = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);
        _logger.LogInformation("Subscribed to topic: {Topic}", topic);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(ct);
                if (result?.Message?.Value == null) continue;

                var typeHeaderBytes = result.Message.Headers.FirstOrDefault(h => h.Key == "eventType")?.GetValueBytes();
                var eventType = typeHeaderBytes != null
                    ? System.Text.Encoding.UTF8.GetString(typeHeaderBytes)
                    : "Unknown";

                await handler(result.Message.Value, eventType);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
            {
                _logger.LogWarning("Topic {Topic} not yet created. Waiting 5s...", topic);
                await Task.Delay(5000, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in consumer loop for {Topic}", topic);
                await Task.Delay(2000, ct);
            }
        }
    }
}

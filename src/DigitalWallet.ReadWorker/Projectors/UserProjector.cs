using DigitalWallet.Modules.Users.Contracts.Events;
using DigitalWallet.Read.Storage.Documents;
using DigitalWallet.Shared.Domain;
using DigitalWallet.Shared.Infrastructure.Interfaces;
using MongoDB.Driver;
using System.Text.Json;

namespace DigitalWallet.ReadWorker.Projectors;

internal class UserProjector(
    IMongoDatabase database,
    IKafkaConsumer consumer,
    ILogger<UserProjector> logger) : BackgroundService
{
    private readonly IMongoCollection<UserDocument> _users = database.GetCollection<UserDocument>("users");
    private readonly IKafkaConsumer _consumer = consumer;
    private readonly ILogger<UserProjector> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("User Projector started. Listening to 'users' domain events...");

        await _consumer.SubscribeAsync(Schemas.Users, "users-projector", async (string json, string typeName) =>
        {
            try
            {
                switch (typeName)
                {
                    case nameof(UserRegisteredIntegrationEvent):
                        var regEvent = Deserialize<UserRegisteredIntegrationEvent>(json);
                        await HandleRegistration(regEvent, stoppingToken);
                        break;

                    default:
                        _logger.LogDebug("Skipping unknown event type: {Type}", typeName);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to project event of type {Type}", typeName);
            }
        }, stoppingToken);
    }

    private async Task HandleRegistration(UserRegisteredIntegrationEvent @event, CancellationToken ct)
    {
        _logger.LogInformation("Projecting registration for: {Email}", @event.Email);

        var update = Builders<UserDocument>.Update
            .Set(x => x.Email, @event.Email)
            .Set(x => x.FirstName, @event.FirstName)
            .Set(x => x.LastName, @event.LastName)
            .SetOnInsert(x => x.RegisteredAt, @event.OccurredOn);

        await _users.UpdateOneAsync(
            x => x.Id == @event.Id,
            update,
            new UpdateOptions { IsUpsert = true },
            ct);
    }

    private static T Deserialize<T>(string json) =>
        JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
}

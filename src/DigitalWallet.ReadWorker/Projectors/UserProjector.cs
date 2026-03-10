using DigitalWallet.Modules.Users.Contracts.Events;
using DigitalWallet.Read.Storage.Documents;
using DigitalWallet.Shared.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace DigitalWallet.ReadWorker.Projectors;

internal class UserProjector : BackgroundService
{
    private readonly IMongoCollection<UserDocument> _users;
    private readonly IKafkaConsumer _consumer;
    private readonly ILogger<UserProjector> _logger;

    public UserProjector(
        IMongoDatabase database,
        IKafkaConsumer consumer,
        ILogger<UserProjector> logger)
    {
        _users = database.GetCollection<UserDocument>("users");
        _consumer = consumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("User Projector started. Listening for UserRegisteredIntegrationEvent...");

        await _consumer.SubscribeAsync<UserRegisteredIntegrationEvent>(async (@event) =>
        {
            _logger.LogInformation("Processing registration for: {Email}", @event.Email);

            var update = Builders<UserDocument>.Update
                .Set(x => x.Email, @event.Email)
                .Set(x => x.FirstName, @event.FirstName)
                .Set(x => x.LastName, @event.LastName)
                .SetOnInsert(x => x.RegisteredAt, @event.OccurredOn);

            await _users.UpdateOneAsync(
                x => x.Id == @event.Id,
                update,
                new UpdateOptions { IsUpsert = true },
                stoppingToken);

        }, stoppingToken);
    }
}

using DigitalWallet.Modules.Users.Contracts.Events;
using DigitalWallet.Read.Storage.Documents;
using MassTransit;
using MongoDB.Driver;

namespace DigitalWallet.ReadWorker.Consumers.Users;

internal class UserCreatedConsumer(
    IMongoDatabase database,
    ILogger<UserCreatedConsumer> logger) : IConsumer<UserCreatedIntegrationEvent>
{
    private readonly IMongoCollection<UserDocument> _users = database.GetCollection<UserDocument>("users");

    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        var @event = context.Message;

        logger.LogInformation("Processing UserCreated event for: {Email} (ID: {Id})", @event.Email, @event.Id);

        var update = Builders<UserDocument>.Update
            .Set(x => x.Email, @event.Email)
            .Set(x => x.FirstName, @event.FirstName)
            .Set(x => x.LastName, @event.LastName)
            .SetOnInsert(x => x.RegisteredAt, @event.OccurredOn);

        await _users.UpdateOneAsync(
            x => x.Id == @event.Id,
            update,
            new UpdateOptions { IsUpsert = true },
            context.CancellationToken);

        logger.LogDebug("Successfully projected User {Id} to Read Model.", @event.Id);
    }
}

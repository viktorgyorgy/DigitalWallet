using DigitalWallet.Modules.Users.Application.Interfaces;
using DigitalWallet.Modules.Users.Contracts.Events;
using DigitalWallet.Modules.Users.Domain.Entities;
using DigitalWallet.Shared.Application;
using Microsoft.Extensions.Logging;

namespace DigitalWallet.Modules.Users.Application.Services;

public class UsersService(
    IUserRepository userRepository,
    IUsersOutboxRepository outboxRepository,
    IUsersUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    ILogger<UsersService> logger)
{
    public async Task CreateProfileAsync(Guid identityId, string email, string firstName, string lastName, CancellationToken ct)
    {
        // Idempotency Check
        if (await userRepository.ExistsAsync(identityId, ct))
        {
            logger.LogWarning("User profile for {Id} already exists. Skipping.", identityId);
            return;
        }

        var user = new User(identityId, email, firstName, lastName, timeProvider.GetUtcNow().UtcDateTime);
        await userRepository.AddAsync(user, ct);

        var @event = new UserCreatedIntegrationEvent(user.Id, user.Email, user.FirstName, user.LastName, Guid.NewGuid(), user.CreatedAt);
        await outboxRepository.AddAsync(OutboxMessage.FromEvent(@event, user.Id));

        await unitOfWork.SaveChangesAsync(ct);
    }
}

using DigitalWallet.Modules.Identity.Contracts;
using DigitalWallet.Modules.Users.Application.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace DigitalWallet.Modules.Users.Infrastructure.Consumers.Identity;

public class UserIdentityCreatedConsumer(
    UsersService usersService,
    ILogger<UserIdentityCreatedConsumer> logger) : IConsumer<UserIdentityCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserIdentityCreatedIntegrationEvent> context)
    {
        var @event = context.Message;

        logger.LogInformation("Processing Identity Created event for {Email} (ID: {Id})",
            @event.Email, @event.Id);

        await usersService.CreateProfileAsync(
            @event.Id,
            @event.Email,
            @event.FirstName,
            @event.LastName,
            context.CancellationToken);

        logger.LogDebug("User profile initialization triggered for {Id}", @event.Id);
    }
}

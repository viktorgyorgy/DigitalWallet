using DigitalWallet.Shared.Domain.Events;

namespace DigitalWallet.Modules.Users.Contracts.Events;

public record UserRegisteredIntegrationEvent(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    Guid EventId,
    DateTimeOffset OccurredOn) : IIntegrationEvent;

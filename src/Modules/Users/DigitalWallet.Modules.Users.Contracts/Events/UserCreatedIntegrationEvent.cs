using DigitalWallet.Shared.Domain.Events;

namespace DigitalWallet.Modules.Users.Contracts.Events;

public record UserCreatedIntegrationEvent(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    Guid EventId,
    DateTimeOffset OccurredOn) : IIntegrationEvent;

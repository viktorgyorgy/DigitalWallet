using DigitalWallet.Shared.Domain.Events;

namespace DigitalWallet.Modules.Identity.Contracts;

public record UserIdentityCreatedIntegrationEvent(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    Guid EventId,
    DateTimeOffset OccurredOn) : IIntegrationEvent;

namespace DigitalWallet.Shared.Domain.Exceptions;

public abstract class NotFoundException(string message) : DomainException(message);
namespace DigitalWallet.Shared.Domain.Exceptions;

public sealed class InvalidRequestException(IDictionary<string, string[]> errors) : DomainException("One or more validation failures have occurred.")
{
    public IDictionary<string, string[]> Errors { get; } = errors;
}

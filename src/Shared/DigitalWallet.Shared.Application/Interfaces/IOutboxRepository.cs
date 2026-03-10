namespace DigitalWallet.Shared.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message);
}
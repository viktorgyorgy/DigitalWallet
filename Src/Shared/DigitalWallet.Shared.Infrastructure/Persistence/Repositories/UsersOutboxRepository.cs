using DigitalWallet.Shared.Application;
using DigitalWallet.Shared.Application.Interfaces;

namespace DigitalWallet.Shared.Infrastructure.Persistence.Repositories;

public abstract class UsersOutboxRepository<TContext> : IOutboxRepository 
    where TContext : BaseDbContext
{
    private readonly TContext _context;

    protected UsersOutboxRepository(TContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxMessage message)
    {
        await _context.Set<OutboxMessage>().AddAsync(message);
    }
}
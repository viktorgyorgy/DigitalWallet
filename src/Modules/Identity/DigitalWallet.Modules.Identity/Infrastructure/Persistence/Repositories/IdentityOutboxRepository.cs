using DigitalWallet.Shared.Application;
using DigitalWallet.Shared.Application.Interfaces;

namespace DigitalWallet.Modules.Identity.Infrastructure.Persistence.Repositories;

internal class IdentityOutboxRepository(IdentityDbContext context) : IOutboxRepository
{
    public async Task AddAsync(OutboxMessage message) => await context.OutboxMessages.AddAsync(message);
}

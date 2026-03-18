using DigitalWallet.Modules.Users.Application.Interfaces;
using DigitalWallet.Shared.Application;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Modules.Users.Infrastructure.Persistence.Repositories;

public class UsersOutboxRepository(UsersDbContext context) : IUsersOutboxRepository
{
    private readonly DbSet<OutboxMessage> _outboxMessages = context.OutboxMessages;
    public async Task AddAsync(OutboxMessage message) => await _outboxMessages.AddAsync(message);
}

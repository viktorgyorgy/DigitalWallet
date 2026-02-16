using DigitalWallet.Modules.Users.Application.Interfaces;

namespace DigitalWallet.Modules.Users.Infrastructure.Persistence;

public class UsersUnitOfWork(UsersDbContext context) : IUsersUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}

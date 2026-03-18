using DigitalWallet.Modules.Users.Application.Interfaces;
using DigitalWallet.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Modules.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await context.Users.AnyAsync(u => u.Id == id, ct);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        => await context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
        => await context.Users.AddAsync(user, ct);
}

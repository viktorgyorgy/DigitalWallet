using DigitalWallet.Modules.Users.Application.Interfaces;
using DigitalWallet.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Modules.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(UsersDbContext context) : IUserRepository
{
    private readonly UsersDbContext _context = context;

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _context.Users.AddAsync(user, ct);
    }
}

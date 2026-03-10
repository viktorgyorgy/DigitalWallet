using DigitalWallet.Modules.Users.Domain.Entities;

namespace DigitalWallet.Modules.Users.Application.Interfaces;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
}
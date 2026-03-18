namespace DigitalWallet.Modules.Users.Application.Interfaces;

public interface IUsersUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

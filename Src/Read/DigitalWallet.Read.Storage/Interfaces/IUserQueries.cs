using DigitalWallet.Read.Storage.Documents;

namespace DigitalWallet.Read.Storage.Interfaces;

public interface IUserQueries
{
    Task<UserDocument?> GetByIdAsync(Guid id, CancellationToken ct = default);
}

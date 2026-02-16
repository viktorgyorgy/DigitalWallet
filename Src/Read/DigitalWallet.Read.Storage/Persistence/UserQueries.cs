using DigitalWallet.Read.Storage.Documents;
using DigitalWallet.Read.Storage.Interfaces;
using MongoDB.Driver;

namespace DigitalWallet.Read.Storage.Persistence;

public class UserQueries(IMongoDatabase database) : IUserQueries
{
    private readonly IMongoCollection<UserDocument> _users = database.GetCollection<UserDocument>("users");

    public async Task<UserDocument?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _users.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
}

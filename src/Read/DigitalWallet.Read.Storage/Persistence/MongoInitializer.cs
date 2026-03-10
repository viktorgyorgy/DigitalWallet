using DigitalWallet.Read.Storage.Documents;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DigitalWallet.Read.Storage.Persistence;

public class MongoDbInitializer(IMongoDatabase database, ILogger<MongoDbInitializer> logger)
{
    private readonly IMongoDatabase _database = database;
    private readonly ILogger<MongoDbInitializer> _logger = logger;

    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("Initializing MongoDB indexes for ReadSide...");

            var users = _database.GetCollection<UserDocument>("users");

            var emailIndex = new CreateIndexModel<UserDocument>(
                Builders<UserDocument>.IndexKeys.Ascending(x => x.Email),
                new CreateIndexOptions { Unique = true, Name = "UX_User_Email" }
            );

            var dateIndex = new CreateIndexModel<UserDocument>(
                Builders<UserDocument>.IndexKeys.Descending(x => x.RegisteredAt),
                new CreateIndexOptions { Name = "IX_User_RegisteredAt" }
            );

            await users.Indexes.CreateManyAsync(new[] { emailIndex, dateIndex });

            _logger.LogInformation("MongoDB indexes initialized successfully.");
        }
        catch (MongoCommandException ex) when (ex.CodeName == "IndexOptionsConflict")
        {
            // This is a common "Senior" catch: 
            // It happens if you try to change an index (like making it Unique) 
            // without dropping the old one first.
            _logger.LogWarning("Index conflict detected. You may need to drop the index manually if you changed its options: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "A fatal error occurred while initializing MongoDB.");
            throw; // Re-throw so the migration container exits with a non-zero code
        }
    }
}

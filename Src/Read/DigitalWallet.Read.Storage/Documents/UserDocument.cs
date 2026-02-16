using MongoDB.Bson.Serialization.Attributes;

namespace DigitalWallet.Read.Storage.Documents;

public class UserDocument
{
    [BsonId]
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    [BsonIgnore]
    public string FullName => $"{FirstName} {LastName}";

    public DateTimeOffset RegisteredAt { get; set; }
}

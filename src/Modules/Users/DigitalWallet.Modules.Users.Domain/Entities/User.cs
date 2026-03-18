namespace DigitalWallet.Modules.Users.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }

    private User() { }

    public User(Guid id, string email, string firstName, string lastName, DateTimeOffset createdAt)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        CreatedAt = createdAt;
    }
}

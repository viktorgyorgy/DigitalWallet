using DigitalWallet.Shared.Domain;

namespace DigitalWallet.Modules.Identity.Domain;

internal class Credential
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Credential() { }

    public Credential(string email, string passwordHash)
    {
        Id = new Guid();
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = Role.User; // Default role
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be empty.");
        PasswordHash = hash;
    }
}

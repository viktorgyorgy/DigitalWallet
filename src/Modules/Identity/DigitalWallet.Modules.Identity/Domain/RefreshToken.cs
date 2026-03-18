namespace DigitalWallet.Modules.Identity.Domain;

internal class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime Expiry { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Guid CredentialId { get; private set; }
    internal Credential Credential { get; private set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= Expiry;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { }

    public RefreshToken(string token, DateTime expiry, Guid credentialId)
    {
        Id = Guid.NewGuid();
        Token = token;
        Expiry = expiry;
        CredentialId = credentialId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revoke() => IsRevoked = true;
}

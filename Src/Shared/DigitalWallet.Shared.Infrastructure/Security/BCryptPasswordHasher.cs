using DigitalWallet.Shared.Application.Interfaces;

namespace DigitalWallet.Shared.Infrastructure.Security;


public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}


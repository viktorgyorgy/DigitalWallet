using DigitalWallet.Modules.Identity.Application.Interfaces;
using DigitalWallet.Modules.Identity.Domain;
using Microsoft.AspNetCore.Identity;

namespace DigitalWallet.Modules.Identity.Infrastructure;

internal class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<Credential> _hasher = new();

    public string Hash(Credential user, string password) =>
        _hasher.HashPassword(user, password);

    public bool Verify(Credential user, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}

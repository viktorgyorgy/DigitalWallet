using DigitalWallet.Modules.Identity.Domain;

namespace DigitalWallet.Modules.Identity.Application.Interfaces;

internal interface IPasswordHasher
{
    string Hash(Credential user, string password);
    bool Verify(Credential user, string password);
}

using DigitalWallet.Modules.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Modules.Identity.Infrastructure.Persistence.Repositories;

internal class CredentialRepository(IdentityDbContext context)
{
    public async Task<Credential?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var normalizedEmail = email.ToLowerInvariant().Trim();

        return await context.Credentials
            .SingleOrDefaultAsync(x => x.Email == normalizedEmail, ct);
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken ct = default)
    {
        var normalizedEmail = email.ToLowerInvariant().Trim();

        return await context.Credentials
            .AnyAsync(x => x.Email == normalizedEmail, ct);
    }

    public async Task AddAsync(Credential credential, CancellationToken ct = default) =>
        await context.Credentials.AddAsync(credential, ct);
}

using DigitalWallet.Modules.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Modules.Identity.Infrastructure.Persistence.Repositories;

internal class RefreshTokenRepository(IdentityDbContext context)
{
    public async Task<RefreshToken?> GetByTokenValueAsync(string token, CancellationToken ct = default)
        => await context.RefreshTokens.Include(t => t.Credential)
    .FirstOrDefaultAsync(t => t.Token == token, ct);

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default)
        => await context.RefreshTokens.AddAsync(refreshToken, ct);
}

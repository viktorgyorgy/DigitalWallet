using DigitalWallet.Modules.Identity.Domain;
using DigitalWallet.Shared.Domain;
using DigitalWallet.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Modules.Identity.Infrastructure.Persistence;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : BaseDbContext(options)
{
    internal DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    internal DbSet<Credential> Credentials => Set<Credential>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Identity);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

using DigitalWallet.Shared.Application;
using DigitalWallet.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Shared.Infrastructure.Persistence;

public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);
        
        modelBuilder.ApplySnakeCaseNames();

        base.OnModelCreating(modelBuilder);
    }
}
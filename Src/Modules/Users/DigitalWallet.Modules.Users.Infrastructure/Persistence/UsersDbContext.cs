using DigitalWallet.Modules.Users.Domain;
using DigitalWallet.Modules.Users.Domain.Entities;
using DigitalWallet.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Modules.Users.Infrastructure.Persistence;

public class UsersDbContext : BaseDbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("users");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
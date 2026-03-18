using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DigitalWallet.Modules.Users.Infrastructure.Persistence;

public class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
{
    public UsersDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=DummyDispenser;Username=postgres;Password=password", o =>
            o.MigrationsHistoryTable("__UsersMigrationsHistory", "users"));

        return new UsersDbContext(optionsBuilder.Options);
    }
}

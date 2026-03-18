using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DigitalWallet.Modules.Identity.Infrastructure.Persistence;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

        // This "Dummy" string tells EF which SQL dialect to use (Postgres).
        // The schema name "identity" matches your Schemas.Identity constant.
        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=MigrationDummy;Username=postgres;Password=password",
            o => o.MigrationsHistoryTable("__IdentityMigrationsHistory", "identity"));

        return new IdentityDbContext(optionsBuilder.Options);
    }
}

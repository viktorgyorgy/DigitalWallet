using DigitalWallet.Modules.Users.Application.Interfaces;
using DigitalWallet.Shared.Infrastructure.Persistence.Repositories;

namespace DigitalWallet.Modules.Users.Infrastructure.Persistence.Repositories;

public class UsersOutboxRepository : UsersOutboxRepository<UsersDbContext>, IUsersOutboxRepository
{
    public UsersOutboxRepository(UsersDbContext context) : base(context) { }
}

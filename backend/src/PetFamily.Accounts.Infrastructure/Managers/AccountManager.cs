using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class AccountManager(AuthorizationDbContext context) : IAccountManager
{
    public async Task CreateAdminAccount(
        AdminAccount account,
        CancellationToken cancellationToken = default)
    {
        await context.Admins.AddAsync(account, cancellationToken);
    }

    public async Task CreateParticipantAccount(
        ParticipantAccount account,
        CancellationToken cancellationToken = default)
    {
        await context.Participants.AddAsync(account, cancellationToken);
    }
}
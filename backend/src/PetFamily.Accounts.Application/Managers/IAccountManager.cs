using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Managers;

public interface IAccountManager
{
    Task CreateAdminAccount(AdminAccount account, CancellationToken cancellationToken = default);

    Task CreateParticipantAccount(ParticipantAccount account, CancellationToken cancellationToken = default);
}
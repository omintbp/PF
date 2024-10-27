using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Managers;

public interface IPermissionManager
{
    Task AddRangeIfNotExists(
        IEnumerable<string> codes,
        CancellationToken cancellationToken);

    Task<Permission?> GetByCode(string code, CancellationToken cancellationToken);

    Task AddPermissionsToRole(
        Role role,
        IEnumerable<string> permissionCodes,
        CancellationToken cancellationToken);

    Task<bool> CheckIfUserHasPermission(Guid userId, string permissionCode, CancellationToken cancellationToken);
}
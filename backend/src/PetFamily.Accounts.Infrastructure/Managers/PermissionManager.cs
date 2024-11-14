using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.DbContexts.Write;

namespace PetFamily.Accounts.Infrastructure.Managers;

public class PermissionManager(AccountWriteDbContext context) : IPermissionManager
{
    public async Task AddRangeIfNotExists(
        IEnumerable<string> codes,
        CancellationToken cancellationToken)
    {
        foreach (var code in codes)
        {
            var isPermissionExist = await context.Permissions.AnyAsync(p =>
                p.Code == code, cancellationToken);

            if (isPermissionExist)
                continue;

            context.Permissions.Add(new Permission(code));
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Permission?> GetByCode(string code, CancellationToken cancellationToken)
    {
        var permission = await context.Permissions.FirstOrDefaultAsync(p =>
            p.Code == code, cancellationToken);

        return permission;
    }

    public async Task AddPermissionsToRole(
        Role role,
        IEnumerable<string> permissionCodes,
        CancellationToken cancellationToken)
    {
        foreach (var permissionCode in permissionCodes)
        {
            var permission = await GetByCode(permissionCode, cancellationToken)
                             ?? throw new ApplicationException(
                                 $"Permission code {permissionCode} not found for role {role.Name}");

            var isAlreadyExists = await context.RolePermissions.AnyAsync(
                rp => rp.PermissionId == permission.Id && rp.RoleId == role.Id,
                cancellationToken);

            if (isAlreadyExists)
                continue;

            var rolePermission = new RolePermission()
            {
                Role = role,
                RoleId = role.Id,
                Permission = permission,
                PermissionId = permission.Id
            };

            await context.RolePermissions.AddAsync(rolePermission, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CheckIfUserHasPermission(
        Guid userId,
        string permissionCode,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            return false;

        var isUserHasPermission = user.Roles.Any(r =>
            r.RolePermissions.Any(rp => rp.Permission.Code == permissionCode));

        return isUserHasPermission;
    }
}
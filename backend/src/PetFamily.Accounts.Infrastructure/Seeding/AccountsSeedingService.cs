using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeedingService
{
    private const string SEEDING_FILE_NAME = "etc/accounts.json";

    private readonly ILogger<AccountsSeedingService> _logger;
    private readonly IPermissionManager _permissionManager;
    private readonly RoleManager<Role> _roleManager;

    public AccountsSeedingService(
        ILogger<AccountsSeedingService> logger,
        IPermissionManager permissionManager,
        RoleManager<Role> roleManager)
    {
        _logger = logger;
        _permissionManager = permissionManager;
        _roleManager = roleManager;
    }

    public async Task Seed(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seeding Accounts...");

        var fileContent = await File.ReadAllTextAsync(SEEDING_FILE_NAME, cancellationToken);

        var accountsOptions = JsonSerializer.Deserialize<AccountsSeedingOptions>(fileContent)
                              ?? throw new NullReferenceException();

        await SeedPermissions(accountsOptions, cancellationToken);
        await SeedRoles(accountsOptions, cancellationToken);
        await SeedRolesPermissions(accountsOptions, cancellationToken);

        _logger.LogInformation("Completed Seeding Accounts...");
    }

    private async Task SeedRoles(
        AccountsSeedingOptions options,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seeding Roles...");

        foreach (var roleToAdd in options.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleToAdd);

            if (role is not null)
                continue;

            await _roleManager.CreateAsync(new Role()
            {
                Name = roleToAdd
            });
        }

        _logger.LogInformation("Completed Seeding Roles...");
    }

    private async Task SeedRolesPermissions(
        AccountsSeedingOptions options,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seeding Roles Permissions...");

        foreach (var roleToSeed in options.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleToSeed)
                       ?? throw new ApplicationException($"Role {roleToSeed} not found");

            await _permissionManager.AddPermissionsToRole(role, options.Roles[roleToSeed], cancellationToken);
        }

        _logger.LogInformation("Completed Seeding Roles...");
    }

    private async Task SeedPermissions(
        AccountsSeedingOptions options,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding Permissions...");

        await _permissionManager.AddRangeIfNotExists(options.Permissions, cancellationToken);

        _logger.LogInformation("Completed Seeding Permissions...");
    }
}
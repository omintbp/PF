using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core.Database;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeedingService
{
    private const string SEEDING_FILE_NAME = "etc/accounts.json";

    private readonly ILogger<AccountsSeedingService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IAccountManager _accountManager;
    private readonly IPermissionManager _permissionManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IServiceScopeFactory _factory;

    public AccountsSeedingService(
        ILogger<AccountsSeedingService> logger,
        [FromKeyedServices(Modules.Accounts)]
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IAccountManager accountManager,
        IPermissionManager permissionManager,
        RoleManager<Role> roleManager,
        IServiceScopeFactory factory)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _accountManager = accountManager;
        _permissionManager = permissionManager;
        _roleManager = roleManager;
        _factory = factory;
    }

    public async Task Seed(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seeding Accounts...");

        var fileContent = await File.ReadAllTextAsync(SEEDING_FILE_NAME, cancellationToken);

        var accountsOptions = JsonSerializer.Deserialize<AccountsSeedingOptions>(fileContent)
                              ?? throw new NullReferenceException();

        await using var scope = _factory.CreateAsyncScope();
        var adminOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdminOptions>>().Value;

        await SeedPermissions(accountsOptions, cancellationToken);
        await SeedRoles(accountsOptions, cancellationToken);
        await SeedRolesPermissions(accountsOptions, cancellationToken);

        await CreateAdminAccountIfNotExist(adminOptions, cancellationToken);

        _logger.LogInformation("Completed Seeding Accounts...");
    }

    private async Task CreateAdminAccountIfNotExist(
        AdminOptions options,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new admin account...");

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var adminUserResult = await _userManager.FindByEmailAsync(options.Email);
            if (adminUserResult != null)
                return;

            var adminRole = await _roleManager.FindByNameAsync(AdminAccount.Admin)
                            ?? throw new ApplicationException("Admin role not found");

            var adminName = FullName.Create(
                options.FirstName,
                options.Surname,
                options.Patronymic).Value;

            var photo = FilePath.None;

            var adminUser = User.CreateAdmin(
                options.UserName,
                options.Email,
                adminName,
                photo,
                adminRole,
                []).Value;
            var adminAccount = new AdminAccount(adminUser.Id, adminUser);

            var createUserResult = await _userManager.CreateAsync(adminUser, options.Password);

            if (createUserResult.Succeeded == false)
                throw new ApplicationException();
            
            await _accountManager.CreateAdminAccount(adminAccount, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex.Message);
        }

        _logger.LogInformation("Admin account created or updated.");

        transaction.Commit();
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
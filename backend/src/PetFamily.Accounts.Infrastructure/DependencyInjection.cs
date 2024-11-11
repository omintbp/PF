using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.Managers;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Core.Database;
using PetFamily.Core.Options;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddAccountSeeder()
            .AddManagers()
            .AddAuthServices(configuration)
            .AddIdentity()
            .AddDbContexts()
            .AddDatabase()
            .AddTokenProvider();
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        return services
            .AddScoped<IPermissionManager, PermissionManager>()
            .AddScoped<IRefreshSessionManager, RefreshSessionManager>()
            .AddScoped<IAccountManager, AccountManager>();
    }

    private static IServiceCollection AddAccountSeeder(this IServiceCollection services)
    {
        return services
            .AddScoped<AccountsSeedingService>()
            .AddSingleton<AccountsSeeder>();
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        return services.AddScoped<AuthorizationDbContext>();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        return services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);
    }

    private static IServiceCollection AddTokenProvider(this IServiceCollection services)
    {
        return services.AddScoped<ITokenProvider, JwtTokenProvider>();
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddAuthServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing jwt configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
            });

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddAuthorization();

        return services;
    }
}
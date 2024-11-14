using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.DbContexts.Write;

public class AccountWriteDbContext(IConfiguration configuration)
    : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Permission> Permissions => Set<Permission>();
    
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<AdminAccount> Admins => Set<AdminAccount>();

    public DbSet<VolunteerAccount> Volunteers => Set<VolunteerAccount>();

    public DbSet<ParticipantAccount> Participants => Set<ParticipantAccount>();
    
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("PetFamily"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .ToTable("users");

        builder.Entity<Role>()
            .ToTable("roles");

        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");

        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");

        builder.ApplyConfigurationsFromAssembly(
            typeof(AccountWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}
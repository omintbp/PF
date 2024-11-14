using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(p => new { p.RoleId, p.PermissionId });

        builder.HasOne(p => p.Role)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(p => p.RoleId);

        builder.HasOne(p => p.Permission)
            .WithMany(p => p.RolesPermissions)
            .HasForeignKey(p => p.PermissionId);
    }
}
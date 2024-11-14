using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Accounts;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class UserRolesDtoConfiguration : IEntityTypeConfiguration<UserRolesDto>
{
    public void Configure(EntityTypeBuilder<UserRolesDto> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(u => new { u.UserId, u.RoleId });
    }
}
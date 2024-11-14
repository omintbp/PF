using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Accounts;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class AdminAccountDtoConfiguration : IEntityTypeConfiguration<AdminAccountDto>
{
    public void Configure(EntityTypeBuilder<AdminAccountDto> builder)
    {
        builder.ToTable("admin_accounts");

        builder.HasKey(a => a.Id);
    }
}
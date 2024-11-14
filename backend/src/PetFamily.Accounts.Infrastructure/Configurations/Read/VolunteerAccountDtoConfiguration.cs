using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Accounts;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class VolunteerAccountDtoConfiguration : IEntityTypeConfiguration<VolunteerAccountDto>
{
    public void Configure(EntityTypeBuilder<VolunteerAccountDto> builder)
    {
        builder.ToTable("volunteer_accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Experience)
            .HasColumnName("experience");

        builder.Property(a => a.Requisites)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<RequisiteDto>>
                    (json, JsonSerializerOptions.Default)!)
            .HasColumnName("requisites");
    }
}
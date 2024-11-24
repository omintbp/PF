using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Domain.AggregateRoot;

namespace PetFamily.VolunteerRequests.Infrastructure.Configurations;

public class VolunteerRequestBanConfiguration : IEntityTypeConfiguration<VolunteerRequestBan>
{
    public void Configure(EntityTypeBuilder<VolunteerRequestBan> builder)
    {
        builder.ToTable("volunteer_request_bans");

        builder.HasKey(x => x.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                id => VolunteerRequestBanId.Create(id)
            );
    }
}
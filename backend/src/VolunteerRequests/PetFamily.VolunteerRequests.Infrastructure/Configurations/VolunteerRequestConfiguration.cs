using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Domain.AggregateRoot;

namespace PetFamily.VolunteerRequests.Infrastructure.Configurations;

public class VolunteerRequestConfiguration : IEntityTypeConfiguration<VolunteerRequest>
{
    public void Configure(EntityTypeBuilder<VolunteerRequest> builder)
    {
        builder.ToTable("volunteer_requests");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                id => VolunteerRequestId.Create(id)
            );

        builder.ComplexProperty(v => v.RejectionComment, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("rejection_comment")
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });
        
        builder.ComplexProperty(v => v.VolunteerInfo, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("volunteer_info")
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });
    }
}
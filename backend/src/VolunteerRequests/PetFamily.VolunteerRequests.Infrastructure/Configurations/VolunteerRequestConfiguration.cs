using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
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
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.Status, pb =>
        {
            pb.Property(p => p.Status)
                .HasColumnName("status")
                .IsRequired();
        });

        builder.OwnsOne(v => v.VolunteerInfo, pb =>
        {
            pb.OwnsOne(p => p.Experience, eb =>
            {
                eb.Property(e => e.Value)
                    .HasColumnName("experience");
            });

            pb.Property(p => p.Requisites)
                .ValueObjectsCollectionJsonConversion(
                    requisite => new RequisiteDto(requisite.Name, requisite.Description),
                    dto => Requisite.Create(dto.Name, dto.Description).Value)
                .HasColumnName("requisites");
        });
    }
}
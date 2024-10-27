using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");
        
        builder.HasKey(a => a.Id);

        builder.ComplexProperty(a => a.Experience, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("experience");
        });
        
        builder.Property(a => a.Requisites)
                .ValueObjectsCollectionJsonConversion(
                    r => new RequisiteDto(r.Name, r.Description),
                    dto => Requisite.Create(dto.Name, dto.Description).Value)
                .HasColumnName("requisites");
    }
}
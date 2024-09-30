using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.Volunteers;
using PetFamily.Domain.PetManagement.Entities;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetPhotoDtoConfiguration : IEntityTypeConfiguration<PetPhotoDto>
{
    public void Configure(EntityTypeBuilder<PetPhotoDto> builder)
    {
        builder.ToTable("pet_photos");

        builder.HasKey(x => x.PhotoId);

        builder.Property(x => x.PhotoId)
            .HasColumnName("id");
    }
}
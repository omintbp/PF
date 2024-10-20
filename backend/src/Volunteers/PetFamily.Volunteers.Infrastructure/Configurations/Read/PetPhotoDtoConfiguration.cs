using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Volunteers;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read;

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
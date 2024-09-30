using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.Species;

namespace PetFamily.Infrastructure.Configurations.Read;

public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.SpeciesId);

        builder.Property(s => s.SpeciesId).HasColumnName("id");

        builder.Property(s => s.SpeciesName).HasColumnName("name");
    }
}
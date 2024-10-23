using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Species;

namespace PetFamily.Species.Infrastructure.Configurations.Read;

public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.BreedId);

        builder.Property(b => b.BreedId).HasColumnName("id");

        builder.Property(b => b.SpeciesId);

        builder.Property(b => b.BreedName).HasColumnName("name");
    }
}
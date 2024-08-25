using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                id => SpeciesId.Create(id));

        builder.ComplexProperty(s => s.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired()
                .HasColumnName("name");
        });

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id");
    }
}
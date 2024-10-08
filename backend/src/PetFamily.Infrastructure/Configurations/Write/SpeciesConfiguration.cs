using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.AggregateRoot;

namespace PetFamily.Infrastructure.Configurations.Write;

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
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey("species_id");
    }
}
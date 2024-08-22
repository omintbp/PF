using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.Species.Breeds;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                id => BreedId.Create(id));

        builder.ComplexProperty(b => b.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name")
                .IsRequired();
        });
    }
}
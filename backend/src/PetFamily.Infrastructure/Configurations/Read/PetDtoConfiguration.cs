using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteers;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name);

        builder.Property(b => b.Description);

        builder.Property(b => b.Phone)
            .HasColumnName("phone_number");

        builder.Property(b => b.Status)
            .HasColumnName("help_status")
            .HasColumnType("varchar");

        builder.Property(b => b.SpeciesId);

        builder.Property(b => b.BreedId);

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.Country);

            ab.Property(a => a.City);

            ab.Property(a => a.Street);

            ab.Property(a => a.House);

            ab.Property(a => a.Flat);
        });

        builder.ComplexProperty(p => p.Details, db =>
        {
            db.Property(d => d.Birthday)
                .HasColumnName("birthday_date");

            db.Property(p => p.Weight)
                .HasColumnName("weight");

            db.Property(p => p.Height)
                .HasColumnName("height");

            db.Property(p => p.HealthInfo)
                .HasColumnName("health_info");

            db.Property(p => p.Color)
                .HasColumnName("color");

            db.Property(p => p.IsCastrated)
                .HasColumnName("is_castrated");

            db.Property(p => p.IsVaccinated)
                .HasColumnName("is_vaccinated");
        });

        builder.Property(i => i.Requisites)
            .HasConversion(
                requisites => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);
    }
}
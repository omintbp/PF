using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                id => PetId.Create(id)
            );

        builder.ComplexProperty(p => p.Name, nb =>
        {
            nb.Property(p => p.Value)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name")
                .IsRequired();
        });

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(p => p.Value)
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.ComplexProperty(p => p.Details, db =>
        {
            db.Property(d => d.Birthday)
                .HasColumnName("birthday_date")
                .IsRequired();

            db.Property(p => p.Weight)
                .HasColumnName("weight");

            db.Property(p => p.Height)
                .HasColumnName("height");

            db.Property(p => p.HealthInfo)
                .HasMaxLength(PetDetails.MAX_HEALTH_INFO_LENGTH)
                .HasColumnName("health_info")
                .IsRequired();

            db.Property(p => p.Color)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("color")
                .IsRequired();

            db.Property(p => p.IsCastrated)
                .HasColumnName("is_castrated");

            db.Property(p => p.IsVaccinated)
                .HasColumnName("is_vaccinated");
        });

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.Country)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            ab.Property(a => a.City)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            ab.Property(a => a.Street)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            ab.Property(a => a.House)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            ab.Property(a => a.Flat)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(phone => phone.Value)
                .HasMaxLength(PhoneNumber.MAX_PHONE_NUMBER_LENGTH)
                .HasColumnName("phone_number")
                .IsRequired();
        });

        builder.Property(p => p.HelpStatus)
            .HasConversion<string>()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(p => p.CreatedAt).HasColumnType("timestamp(6)");

        builder.OwnsOne(p => p.PaymentDetails, pb =>
        {
            pb.ToJson("payment_details");

            pb.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

                rb.Property(r => r.Name)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .IsRequired();
            });
        });

        builder.ComplexProperty(p => p.SpeciesDetails, db =>
        {
            db.Property(d => d.SpeciesId)
                .HasConversion(
                    id => id.Value,
                    id => SpeciesId.Create(id))
                .HasColumnName("species_id");

            db.Property(d => d.BreedId)
                .HasColumnName("breed_id");
        });

        builder.HasMany(p => p.Photos)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction)
            .HasForeignKey("pet_id");
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

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

        builder.ComplexProperty(pet => pet.Position, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("position");
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
            pb.Property(details => details.Requisites)
                .ValueObjectsCollectionJsonConversion(
                    r => new RequisiteDto(r.Name, r.Description),
                    dto => Requisite.Create(dto.Name, dto.Description).Value)
                .HasColumnName("requisites")
                .IsRequired(false);
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
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey("pet_id");
    }
}
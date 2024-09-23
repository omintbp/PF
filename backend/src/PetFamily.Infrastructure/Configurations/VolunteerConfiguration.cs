using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.Shared;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value)
            );

        builder.ComplexProperty(v => v.FullName, nb =>
        {
            nb.Property(n => n.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("first_name");

            nb.Property(n => n.Patronymic)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("patronymic");

            nb.Property(n => n.Surname)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("surname");
        });

        builder.ComplexProperty(v => v.Email, eb =>
        {
            eb.Property(e => e.Value)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .HasColumnName("description")
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.Experience, eb =>
        {
            eb.Property(d => d.Value)
                .HasColumnName("experience");
        });

        builder.ComplexProperty(v => v.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("phone_number")
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();
        });

        builder.OwnsOne(v => v.Requisites, vb =>
        {
            vb.Property(r => r.Values)
                .ValueObjectsCollectionJsonConversion(
                    r => new RequisiteDto(r.Name, r.Description),
                    dto => Requisite.Create(dto.Name, dto.Description).Value)
                .HasColumnName("requisites");
        });

        builder.OwnsOne(v => v.SocialNetworks, sb =>
        {
            sb.Property(s => s.Values)
                .ValueObjectsCollectionJsonConversion(
                    sn => new SocialNetworkDto(sn.Url, sn.Name),
                    dto => SocialNetwork.Create(dto.Url, dto.Name).Value)
                .HasColumnName("social_networks");
        });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey("volunteer_id");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}
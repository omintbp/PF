using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.Core.DTOs.Shared;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ComplexProperty(u => u.Photo, pb =>
        {
            pb.Property(p => p.Path)
                .HasColumnName("photo");
        });

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

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

        builder
            .HasOne(u => u.ParticipantAccount)
            .WithOne(a => a.User)
            .HasForeignKey<ParticipantAccount>(u => u.UserId)
            .IsRequired(false);

        builder
            .HasOne(u => u.VolunteerAccount)
            .WithOne(a => a.User)
            .HasForeignKey<VolunteerAccount>(u => u.UserId)
            .IsRequired(false);

        builder
            .HasOne(u => u.AdminAccount)
            .WithOne(a => a.User)
            .HasForeignKey<AdminAccount>(a => a.UserId)
            .IsRequired(false);

        builder.Property(s => s.SocialsNetworks)
            .ValueObjectsCollectionJsonConversion(
                sn => new SocialNetworkDto(sn.Url, sn.Name),
                dto => SocialNetwork.Create(dto.Url, dto.Name).Value)
            .HasColumnName("social_networks");
    }
}
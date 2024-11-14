using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.DTOs.Accounts;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.ComplexProperty(v => v.FullName, nb =>
        {
            nb.Property(n => n.FirstName)
                .HasColumnName("first_name");

            nb.Property(n => n.Patronymic)
                .HasColumnName("patronymic");

            nb.Property(n => n.Surname)
                .HasColumnName("surname");
        });

        builder
            .HasOne(u => u.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccountDto>(u => u.UserId)
            .IsRequired(false);

        builder
            .HasOne(u => u.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccountDto>(u => u.UserId)
            .IsRequired(false);

        builder
            .HasOne(u => u.AdminAccount)
            .WithOne()
            .HasForeignKey<AdminAccountDto>(a => a.UserId)
            .IsRequired(false);

        builder.ComplexProperty(u => u.Photo, pb =>
        {
            pb.Property(p => p.Path)
                .HasColumnName("photo");
        });

        builder.Property(u => u.SocialNetworks)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<SocialNetworkDto>>
                    (json, JsonSerializerOptions.Default)!);

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<UserRolesDto>(
                r =>
                    r.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId),
                l =>
                    l.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId)
            );
    }
}
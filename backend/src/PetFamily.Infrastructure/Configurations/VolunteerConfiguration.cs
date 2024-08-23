using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.Volunteers;
using PetFamily.Domain.Shared;

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

        builder.OwnsOne(v => v.Details, vb =>
        {
            vb.ToJson("details");
            
            vb.OwnsMany(p => p.Requisites, rb =>
            {
                rb.Property(r => r.Name)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .IsRequired();

                rb.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                    .IsRequired(false);
            });
            
            vb.OwnsMany(p => p.SocialNetworks, rb =>
            {
                rb.Property(r => r.Name)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .IsRequired();
                
                rb.Property(r => r.Url)
                    .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                    .IsRequired();
            });
        });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction)
            .HasForeignKey("volunteer_id");
    }
}
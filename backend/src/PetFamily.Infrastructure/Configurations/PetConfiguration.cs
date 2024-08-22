using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.Pets;
using PetFamily.Domain.Shared;

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
        
        builder.Property(p => p.Name)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.Species)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        
        builder.Property(p => p.Breed)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.Color)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.HealthInfo)
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
            .IsRequired();

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

        builder.Property(p => p.Weight);

        builder.Property(p => p.Height);

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(phone => phone.Value)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("phone_number")
                .IsRequired();
        });

        builder.Property(p => p.IsCastrated);
        
        builder.Property(p => p.Birthday).IsRequired();
        
        builder.Property(p => p.IsVaccinated);
        
        builder.Property(p => p.HelpStatus).HasConversion<string>()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.CreatedAt);
        
        builder.OwnsOne(p => p.PaymentDetails, pb =>
        {
            pb.ToJson();

            pb.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(r => r.Description)
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

                rb.Property(r => r.Name)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                    .IsRequired();
            });
        });

        builder.HasMany(p => p.Photos)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction)
            .HasForeignKey("pet_id");
    }
}
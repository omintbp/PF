using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Infrastructure.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                id => MessageId.Create(id)
            );

        builder.ComplexProperty(m => m.Text, pb =>
        {
            pb.Property(t => t.Value)
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .IsRequired(true)
                .HasColumnName("text");
        });
    }
}
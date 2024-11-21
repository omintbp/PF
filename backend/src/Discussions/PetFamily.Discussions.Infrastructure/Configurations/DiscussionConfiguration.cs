using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Extensions;
using PetFamily.Discussions.Domain.AggregateRoot;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Infrastructure.Configurations;

public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable("discussions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                id => DiscussionId.Create(id)
            );

        builder.Property(d => d.Users)
            .ValueObjectsCollectionJsonConversion(
                r => r,
                r => r)
            .HasColumnName("users");

        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey("discussion_id");
    }
}
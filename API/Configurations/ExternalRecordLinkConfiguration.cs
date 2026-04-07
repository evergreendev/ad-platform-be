using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class ExternalRecordLinkConfiguration : IEntityTypeConfiguration<ExternalRecordLink>
{
    public void Configure(EntityTypeBuilder<ExternalRecordLink> builder)
    {
        builder.ToTable("external_record_link");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.InternalEntityType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.InternalEntityId)
            .IsRequired();

        builder.Property(x => x.ExternalEntityType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ExternalId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.SyncStatus)
            .HasMaxLength(50);

        builder.Property(x => x.LastSyncError)
            .HasMaxLength(4000);

        builder.Property(x => x.ProviderMetadataJson)
            .HasColumnType("jsonb");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => new
            {
                x.IntegrationConnectionId,
                x.InternalEntityType,
                x.InternalEntityId,
                x.ExternalEntityType
            })
            .IsUnique();

        builder.HasIndex(x => new
            {
                x.IntegrationConnectionId,
                x.ExternalEntityType,
                x.ExternalId
            })
            .IsUnique();

        builder.HasMany(x => x.SyncLogs)
            .WithOne(x => x.ExternalRecordLink)
            .HasForeignKey(x => x.ExternalRecordLinkId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
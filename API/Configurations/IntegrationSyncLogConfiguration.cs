using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class IntegrationSyncLogConfiguration : IEntityTypeConfiguration<IntegrationSyncLog>
{
    public void Configure(EntityTypeBuilder<IntegrationSyncLog> builder)
    {
        builder.ToTable("integration_sync_log");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Direction)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Message)
            .HasMaxLength(4000);

        builder.Property(x => x.PayloadJson)
            .HasColumnType("jsonb");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.IntegrationConnectionId);
        builder.HasIndex(x => x.ExternalRecordLinkId);
        builder.HasIndex(x => x.CreatedAt);
    }
}
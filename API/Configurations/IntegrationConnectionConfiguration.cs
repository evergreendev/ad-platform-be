using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class IntegrationConnectionConfiguration : IEntityTypeConfiguration<IntegrationConnection>
{
    public void Configure(EntityTypeBuilder<IntegrationConnection> builder)
    {
        builder.ToTable("integration_connection");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Provider)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(50);

        builder.Property(x => x.AuthType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.MetadataJson)
            .HasColumnType("jsonb");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => new { x.Provider, x.DisplayName })
            .IsUnique();

        builder.HasMany(x => x.ExternalRecordLinks)
            .WithOne(x => x.IntegrationConnection)
            .HasForeignKey(x => x.IntegrationConnectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SyncLogs)
            .WithOne(x => x.IntegrationConnection)
            .HasForeignKey(x => x.IntegrationConnectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
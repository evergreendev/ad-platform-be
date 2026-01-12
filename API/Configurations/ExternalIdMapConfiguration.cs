using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class ExternalIdMapConfiguration : IEntityTypeConfiguration<ExternalIdMap>
{
    public void Configure(EntityTypeBuilder<ExternalIdMap> builder)
    {
        builder.ToTable("external_id_map");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SourceSystem)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SourceTable)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SourceId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.InternalId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        //This makes imports idempotent
        builder.HasIndex(x => new
            {
                x.SourceSystem,
                x.SourceTable,
                x.SourceId
            })
            .IsUnique();
    }
}
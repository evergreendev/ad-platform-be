using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class MergeFieldConfiguration : IEntityTypeConfiguration<MergeField>
{
    public void Configure(EntityTypeBuilder<MergeField> builder)
    {
        builder.ToTable("merge_field");

        builder.HasKey(x => x.Key);

        builder.Property(x => x.Key)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.Label)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.Entity)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.DataType)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasMany(x => x.TemplateFields)
            .WithOne(x => x.MergeField)
            .HasForeignKey(x => x.MergeFieldKey)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

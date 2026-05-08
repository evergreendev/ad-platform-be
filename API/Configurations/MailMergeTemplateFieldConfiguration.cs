using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class MailMergeTemplateFieldConfiguration : IEntityTypeConfiguration<MailMergeTemplateField>
{
    public void Configure(EntityTypeBuilder<MailMergeTemplateField> builder)
    {
        builder.ToTable("mail_merge_template_field");

        builder.HasKey(x => new { x.TemplateId, x.MergeFieldKey });

        builder.Property(x => x.TemplateId)
            .IsRequired();

        builder.Property(x => x.MergeFieldKey)
            .IsRequired()
            .HasColumnType("text");

        builder.HasOne(x => x.Template)
            .WithMany(x => x.TemplateFields)
            .HasForeignKey(x => x.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.MergeField)
            .WithMany(x => x.TemplateFields)
            .HasForeignKey(x => x.MergeFieldKey)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

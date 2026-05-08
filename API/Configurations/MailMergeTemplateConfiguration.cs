using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class MailMergeTemplateConfiguration : IEntityTypeConfiguration<MailMergeTemplate>
{
    public void Configure(EntityTypeBuilder<MailMergeTemplate> builder)
    {
        builder.ToTable("mail_merge_template");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.SubjectTemplate)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.BodyJson)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(x => x.BodyHtml)
            .HasColumnType("text");

        builder.Property(x => x.PlainText)
            .HasColumnType("text");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasMany(x => x.TemplateFields)
            .WithOne(x => x.Template)
            .HasForeignKey(x => x.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class EmailMessageConfiguration : IEntityTypeConfiguration<EmailMessage>
{
    public void Configure(EntityTypeBuilder<EmailMessage> builder)
    {
        builder.ToTable("email_message");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ToEmailAddress)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.ToDisplayName)
            .HasColumnType("text");

        builder.Property(x => x.Subject)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.BodyHtml)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.PlainText)
            .HasColumnType("text");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Provider)
            .HasColumnType("text");

        builder.Property(x => x.ProviderMessageId)
            .HasColumnType("text");

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasOne(x => x.Campaign)
            .WithMany(x => x.EmailMessages)
            .HasForeignKey(x => x.CampaignId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Contact)
            .WithMany(x => x.EmailMessages)
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.CompanyContact)
            .WithMany(x => x.EmailMessages)
            .HasForeignKey(x => x.CompanyContactId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.MailMergeTemplate)
            .WithMany(x => x.EmailMessages)
            .HasForeignKey(x => x.MailMergeTemplateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Events)
            .WithOne(x => x.EmailMessage)
            .HasForeignKey(x => x.EmailMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Links)
            .WithOne(x => x.EmailMessage)
            .HasForeignKey(x => x.EmailMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CampaignId);
        builder.HasIndex(x => x.ContactId);
        builder.HasIndex(x => x.CompanyContactId);
        builder.HasIndex(x => x.MailMergeTemplateId);
        builder.HasIndex(x => x.ProviderMessageId);
        builder.HasIndex(x => x.Status);
    }
}

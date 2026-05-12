using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class EmailLinkConfiguration : IEntityTypeConfiguration<EmailLink>
{
    public void Configure(EntityTypeBuilder<EmailLink> builder)
    {
        builder.ToTable("email_link");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmailMessageId)
            .IsRequired();

        builder.Property(x => x.OriginalUrl)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.Label)
            .IsRequired()
            .HasColumnType("text");

        builder.HasOne(x => x.EmailMessage)
            .WithMany(x => x.Links)
            .HasForeignKey(x => x.EmailMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.EmailMessageId);
    }
}

using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class EmailEventConfiguration : IEntityTypeConfiguration<EmailEvent>
{
    public void Configure(EntityTypeBuilder<EmailEvent> builder)
    {
        builder.ToTable("email_event");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.OccurredAt)
            .IsRequired();

        builder.Property(x => x.ReceivedAt);

        builder.Property(x => x.ProviderEventId)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(x => x.MetadataJson)
            .HasColumnType("jsonb");

        builder.HasOne(x => x.EmailMessage)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.EmailMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.EmailMessageId);
        builder.HasIndex(x => x.ProviderEventId);
        builder.HasIndex(x => x.EventType);
        builder.HasIndex(x => x.OccurredAt);
    }
}

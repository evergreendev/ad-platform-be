using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class CampaignActivityEventConfiguration : IEntityTypeConfiguration<CampaignActivityEvent>
{
    public void Configure(EntityTypeBuilder<CampaignActivityEvent> builder)
    {
        builder.ToTable("campaign_activity_events");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasOne(x => x.CampaignActivity)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.CampaignActivityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

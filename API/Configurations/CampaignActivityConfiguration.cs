using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class CampaignActivityConfiguration : IEntityTypeConfiguration<CampaignActivity>
{
    public void Configure(EntityTypeBuilder<CampaignActivity> builder)
    {
        builder.ToTable("campaign_activities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ActivityType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasOne(x => x.CampaignContact)
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.CampaignContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Events)
            .WithOne(x => x.CampaignActivity)
            .HasForeignKey(x => x.CampaignActivityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

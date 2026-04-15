using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class CampaignContactConfiguration : IEntityTypeConfiguration<CampaignContact>
{
    public void Configure(EntityTypeBuilder<CampaignContact> builder)
    {
        builder.ToTable("campaign_contacts");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Campaign)
            .WithMany(x => x.CampaignContacts)
            .HasForeignKey(x => x.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Activities)
            .WithOne(x => x.CampaignContact)
            .HasForeignKey(x => x.CampaignContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CampaignId, x.ContactId })
            .IsUnique();
    }
}

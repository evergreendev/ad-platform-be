using API.Models;

namespace API.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("contact");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Basic Fields
        builder.Property(x => x.FirstName)
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasMaxLength(100);

        builder.Property(x => x.Salutation)
            .HasMaxLength(50);

        builder.Property(x => x.JobTitle)
            .HasMaxLength(150);

        builder.Property(x => x.Department)
            .HasMaxLength(150);

        builder.Property(x => x.Gender)
            .HasMaxLength(50);

        // Address
        builder.Property(x => x.AddressLine1)
            .HasMaxLength(200);

        builder.Property(x => x.AddressLine2)
            .HasMaxLength(200);

        builder.Property(x => x.City)
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasMaxLength(50);

        builder.Property(x => x.Zip)
            .HasMaxLength(20);

        builder.Property(x => x.Country)
            .HasMaxLength(100);

        // CRM fields
        builder.Property(x => x.LeadSource)
            .HasMaxLength(100);

        builder.Property(x => x.LeadStatus)
            .HasMaxLength(100);

        builder.Property(x => x.HubspotId)
            .HasMaxLength(100);

        builder.Property(x => x.UserRepId)
            .HasMaxLength(100);

        // Flags
        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Dates
        builder.Property(x => x.CreatedDate)
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.LastUpdatedDate)
            .HasColumnType("timestamp with time zone");

        // Relationships

        builder.HasMany(x => x.CompanyContacts)
            .WithOne(x => x.Contact)
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Emails)
            .WithOne(x => x.Contact)
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes (important for CRM performance)

        builder.HasIndex(x => x.LastName);
        builder.HasIndex(x => x.FirstName);
        builder.HasIndex(x => x.IsActive);

        builder.HasIndex(x => x.HubspotId);
        builder.HasIndex(x => x.UserRepId);

        builder.HasIndex(x => new { x.FirstName, x.LastName });
        
        builder.HasIndex(x => new { x.LastName, x.FirstName, x.City });
    }
}
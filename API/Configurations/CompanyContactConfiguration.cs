using API.Models;

namespace API.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class CompanyContactConfiguration : IEntityTypeConfiguration<CompanyContact>
{
    public void Configure(EntityTypeBuilder<CompanyContact> builder)
    {
        builder.ToTable("company_contact");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.StartDate)
            .HasColumnType("timestamp");

        builder.Property(x => x.EndDate)
            .HasColumnType("timestamp");

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.CompanyContacts)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Contact)
            .WithMany(x => x.CompanyContacts)
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Roles)
            .WithOne(x => x.CompanyContact)
            .HasForeignKey(x => x.CompanyContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Emails)
            .WithOne(x => x.CompanyContact)
            .HasForeignKey(x => x.CompanyContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CompanyId);
        builder.HasIndex(x => x.ContactId);

        builder.HasIndex(x => new { x.CompanyId, x.ContactId })
            .IsUnique();

        builder.HasIndex(x => new { x.CompanyId, x.IsPrimary });
    }
}
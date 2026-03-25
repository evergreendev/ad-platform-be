using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class CompanyContactEmailConfiguration : IEntityTypeConfiguration<CompanyContactEmail>
{
    public void Configure(EntityTypeBuilder<CompanyContactEmail> builder)
    {
        builder.ToTable("company_contact_email");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DoNotEmail)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(x => x.CompanyContact)
            .WithMany(x => x.Emails)
            .HasForeignKey(x => x.CompanyContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CompanyContactId);
        builder.HasIndex(x => x.Email);
    }
}

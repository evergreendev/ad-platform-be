using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class CompanyContactPhoneConfiguration : IEntityTypeConfiguration<CompanyContactPhone>
{
    public void Configure(EntityTypeBuilder<CompanyContactPhone> builder)
    {
        builder.ToTable("company_contact_phone");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DoNotCall)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(x => x.CompanyContact)
            .WithMany(x => x.Phones)
            .HasForeignKey(x => x.CompanyContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CompanyContactId);
        builder.HasIndex(x => new { x.Phone, x.CompanyContactId }).IsUnique();
    }
}

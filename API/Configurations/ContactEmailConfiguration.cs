using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class ContactEmailConfiguration : IEntityTypeConfiguration<ContactEmail>
{
    public void Configure(EntityTypeBuilder<ContactEmail> builder)
    {
        builder.ToTable("contact_email");

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

        builder.HasOne(x => x.Contact)
            .WithMany(x => x.Emails)
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ContactId);
        builder.HasIndex(x => x.Email);
    }
}

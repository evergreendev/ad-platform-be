using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class CompanyConfiguration: IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("company");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.AssociatedCompany)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.WebsiteUrl).HasMaxLength(500);
        
        builder.Property(x => x.Type).HasMaxLength(50);
        
        builder.Property(x => x.Address).HasMaxLength(200);
        
        builder.Property(x => x.Address2).HasMaxLength(200);
        
        builder.Property(x => x.City).HasMaxLength(100);
        
        builder.Property(x => x.State).HasMaxLength(50);
        
        builder.Property(x => x.Zip).HasMaxLength(20);
        
        builder.Property(x => x.Country).HasMaxLength(100);
        
        builder.Property(x => x.TaxId).HasMaxLength(64);

        builder.Property(x => x.PrimaryRepName).HasMaxLength(200);
        builder.Property(x => x.LegacyBillingContact).HasMaxLength(200);
        builder.Property(x => x.LegacyPrimaryContact).HasMaxLength(200);
        builder.Property(x => x.LegacyPrimaryCategory).HasMaxLength(200);
    }
}
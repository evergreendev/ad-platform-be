using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class CompanyContactRoleConfiguration : IEntityTypeConfiguration<CompanyContactRole>
{
    public void Configure(EntityTypeBuilder<CompanyContactRole> builder)
    {
        builder.ToTable("company_contact_role");

        builder.HasKey(x => new { x.CompanyContactId, x.RoleId });

        builder.HasOne(x => x.CompanyContact)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.CompanyContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.CompanyContactRoles)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

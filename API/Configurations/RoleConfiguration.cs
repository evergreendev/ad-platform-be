using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(x => x.CompanyContactRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(
            new Role{Id = 1, Name = "Billing", IsSystem = true},
            new Role{Id = 2, Name = "Print Artwork", IsSystem = true},
            new Role{Id = 3, Name = "Digital Artwork", IsSystem = true},
            new Role{Id = 4, Name = "Marketing", IsSystem = true}
                );
    }
}

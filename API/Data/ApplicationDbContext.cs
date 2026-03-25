using API.Configurations;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ExternalIdMap> ExternalIdMaps => Set<ExternalIdMap>();
    public DbSet<Company> Companies => Set<Company>();
    
    public DbSet<Contact> Contacts => Set<Contact>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ExternalIdMapConfiguration());
        
        builder.ApplyConfiguration(new CompanyConfiguration());
        
        builder.ApplyConfiguration(new ContactConfiguration());
        
        base.OnModelCreating(builder);
    }
}
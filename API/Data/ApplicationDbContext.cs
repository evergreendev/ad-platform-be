using API.Configurations;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ExternalIdMap> ExternalIdMaps => Set<ExternalIdMap>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ExternalIdMapConfiguration());
        
        base.OnModelCreating(builder);
    }
}
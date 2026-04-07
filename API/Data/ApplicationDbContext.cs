using API.Configurations;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ExternalIdMap> ExternalIdMaps => Set<ExternalIdMap>();
    public DbSet<Company> Companies => Set<Company>();
    
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<CompanyContact> CompanyContacts => Set<CompanyContact>();
    public DbSet<CompanyContactEmail> CompanyContactEmails => Set<CompanyContactEmail>();
    public DbSet<CompanyContactPhone> CompanyContactPhones => Set<CompanyContactPhone>();
    public DbSet<CompanyContactRole> CompanyContactRoles => Set<CompanyContactRole>();
    public DbSet<ContactEmail> ContactEmails => Set<ContactEmail>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<IntegrationConnection> IntegrationConnections => Set<IntegrationConnection>();
    public DbSet<ExternalRecordLink> ExternalRecordLinks => Set<ExternalRecordLink>();
    public DbSet<IntegrationSyncLog> IntegrationSyncLogs => Set<IntegrationSyncLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ExternalIdMapConfiguration());
        builder.ApplyConfiguration(new CompanyConfiguration());
        builder.ApplyConfiguration(new ContactConfiguration());
        builder.ApplyConfiguration(new ContactEmailConfiguration());
        builder.ApplyConfiguration(new CompanyContactConfiguration());
        builder.ApplyConfiguration(new CompanyContactEmailConfiguration());
        builder.ApplyConfiguration(new CompanyContactPhoneConfiguration());
        builder.ApplyConfiguration(new CompanyContactRoleConfiguration());
        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new IntegrationConnectionConfiguration());
        builder.ApplyConfiguration(new ExternalRecordLinkConfiguration());
        builder.ApplyConfiguration(new IntegrationSyncLogConfiguration());

        base.OnModelCreating(builder);
    }
}
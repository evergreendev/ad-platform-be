namespace API.Models;

public class CompanyContact
{
    public Guid Id { get; set; }
    
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    
    public Guid ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
    
    public bool IsPrimary { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string? Notes { get; set; }
    public ICollection<CompanyContactRole> Roles { get; set; } = new List<CompanyContactRole>();
    public ICollection<CompanyContactEmail> Emails { get; set; } = new List<CompanyContactEmail>();
    public ICollection<CompanyContactPhone> Phones { get; set; } = new List<CompanyContactPhone>();
}
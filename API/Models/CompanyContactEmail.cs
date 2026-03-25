namespace API.Models;

public class CompanyContactEmail
{
    public Guid Id { get; set; }

    public Guid CompanyContactId { get; set; }
    public CompanyContact CompanyContact { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public bool IsPrimary { get; set; }
    public bool DoNotEmail { get; set; }
}
namespace API.Models;

public class CompanyContactPhone
{
    public Guid Id { get; set; }

    public Guid CompanyContactId { get; set; }
    public CompanyContact CompanyContact { get; set; } = null!;

    public string Phone { get; set; } = null!;
    
    public bool IsPrimary { get; set; }
    public bool DoNotCall { get; set; }
}
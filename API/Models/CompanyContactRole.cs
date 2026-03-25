namespace API.Models;

public class CompanyContactRole
{
    public Guid CompanyContactId { get; set; }
    public CompanyContact CompanyContact { get; set; } = null!;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
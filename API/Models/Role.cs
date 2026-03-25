namespace API.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<CompanyContactRole> CompanyContactRoles { get; set; } = new List<CompanyContactRole>();
}
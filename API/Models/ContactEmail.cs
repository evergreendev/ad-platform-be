namespace API.Models;

public class ContactEmail
{
    public Guid Id { get; set; }
    
    public Guid ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public bool IsPrimary { get; set; }
    public bool DoNotEmail { get; set; }
}
namespace Auth.Models;

public class ReadUserDto
{
    public required string Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
namespace API.DTOs;

public class ReadUserDTO
{
    public required string Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
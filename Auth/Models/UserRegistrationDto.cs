namespace Auth.Models;

public class UserRegistrationDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
}

public class UserRegistrationResponseDto
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
}

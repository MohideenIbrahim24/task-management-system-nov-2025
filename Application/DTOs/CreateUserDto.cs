// Application/DTOs/CreateUserDto.cs
public class CreateUserDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User"; // Admin or User
}

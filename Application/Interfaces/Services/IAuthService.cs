using Domain.Entities;

public interface IAuthService
{
    string GenerateJwtToken(User user);
    Task<User?> ValidateUserAsync(string username, string password);
    Task<User> CreateUserAsync(CreateUserDto dto);
}

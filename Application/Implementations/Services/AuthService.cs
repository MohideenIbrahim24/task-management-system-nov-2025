using Application.Interfaces;
using Domain.Entities;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtService _jwtService;

    public AuthService(IUnitOfWork uow, IJwtService jwtService)
    {
        _uow = uow; // only interfaces, no EF dependency
        _jwtService = jwtService;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await _uow.Users.GetByUserNameAsync(username);
        if (user == null) return null;

        // validate password here 
        return  BCrypt.Net.BCrypt.Verify(password, user.UserPasswordHash) ? user : null;
    }

    public string GenerateJwtToken(User user) => _jwtService.GenerateToken(user);
    public async Task<User> CreateUserAsync(CreateUserDto dto)
    {
        // Check if username exists
        var exists = await _uow.Users.GetByUserNameAsync(dto.UserName);
                
        if (exists != null)
            throw new Exception("Username already exists");

        var user = new User { UserName = dto.UserName, UserPasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), Role = dto.Role };
        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();
        return user;
    }

}

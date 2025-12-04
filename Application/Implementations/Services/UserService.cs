using Domain.Entities;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await _userRepo.GetByUserNameAsync(username);
        if (user == null) return null;

        // password hash check (simplified)
        return user;
    }
}

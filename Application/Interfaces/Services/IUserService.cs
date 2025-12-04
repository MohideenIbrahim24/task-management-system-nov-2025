using Domain.Entities;

public interface IUserService
{
    Task<User?> ValidateUserAsync(string username, string password);
}

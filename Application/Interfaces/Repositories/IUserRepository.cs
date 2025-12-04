using Domain.Entities;
using todolistapp.Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUserNameAsync(string username);
}

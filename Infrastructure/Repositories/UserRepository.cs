using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext ctx) : base(ctx) { }

    public Task<User?> GetByUserNameAsync(string username)
        => _context.Users.FirstOrDefaultAsync(u => u.UserName == username);


}

using Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public ITaskRepository Tasks { get; }

    public UnitOfWork(AppDbContext ctx)
    {
        _context = ctx;
        Users = new UserRepository(ctx);
        Tasks = new TaskRepository(ctx);
    }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}

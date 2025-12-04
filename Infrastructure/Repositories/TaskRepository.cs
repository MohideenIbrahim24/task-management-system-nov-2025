using Domain.Entities;
using Domain.Enum;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedResult<TaskItem>> GetPagedAsync(int? userId, TaskProgress? status, DateTime? from, DateTime? to, string? search, int page, int pageSize)
    {
        var q = _context.Tasks.Include(t => t.AssignedTo).AsQueryable();

        if (userId.HasValue) q = q.Where(t => t.AssignedToId == userId.Value);
        if (status.HasValue) q = q.Where(t => t.Status == status.Value);
        if (from.HasValue) q = q.Where(t => t.DueDate >= from.Value);
        if (to.HasValue) q = q.Where(t => t.DueDate <= to.Value);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));

        var total = await q.CountAsync();

        var items = await q.OrderBy(t => t.DueDate)
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();

        return new PagedResult<TaskItem> { Items = items, TotalCount = total };
    }

    public Task<TaskItem?> GetAsync(int id) => _context.Tasks.Include(t => t.AssignedTo).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<List<TaskItem>> FilterAsync(int? userId, TaskProgress? status)
    {
        var query = _context.Tasks.AsQueryable();

        if (userId.HasValue)
            query = query.Where(x => x.AssignedToId == userId);

        if (status.HasValue)
            query = query.Where(x => x.Status == status);

        return await query.ToListAsync(); // EF Core async call
    }

}

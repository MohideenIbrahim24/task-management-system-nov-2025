using Domain.Entities;
using Domain.Enum;
using todolistapp.Application.Interfaces;

public interface ITaskRepository : IGenericRepository<TaskItem>
{
    Task<PagedResult<TaskItem>> GetPagedAsync(int? userId, TaskProgress? status, DateTime? from, DateTime? to, string? search, int page, int pageSize);
    Task<TaskItem?> GetAsync(int id);
    Task<List<TaskItem>> FilterAsync(int? userId, TaskProgress? status);
    //    IQueryable<TaskItem> Query();
}

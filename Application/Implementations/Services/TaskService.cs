using Application.DTOs;
using Domain.Entities;
using Domain.Enum;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cache;

    public TaskService(IUnitOfWork uow, ICacheService cache)
    {
        _uow = uow;
        _cache = cache;
    }

    public async Task<List<TaskItem>> GetAllTasksAsync() =>
        await _uow.Tasks.GetAllAsync();

    public async Task<TaskItem> CreateTaskAsync(CreateTaskDto dto)
    {
        var item = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            AssignedToId = dto.AssignedToUserId
        };

        await _uow.Tasks.AddAsync(item);
        await _uow.SaveChangesAsync();
        return item;
    }

    public async Task<List<TaskItem>> GetFilteredTasksAsync(int? userId, TaskProgress? status)
    {
        return await _uow.Tasks.FilterAsync(userId, status);
    }

    private string CacheKey(int? userId, TaskProgress? status, DateTime? from, DateTime? to, string? search, int page, int pageSize)
        => $"tasks:{userId}:{status}:{from}:{to}:{search}:{page}:{pageSize}";

    public async Task<PagedResult<TaskDto>> GetTasksAsync(int? userId, TaskProgress? status, DateTime? from, DateTime? to, string? search, int page, int pageSize)
    {
        var key = CacheKey(userId, status, from, to, search, page, pageSize);

       if (_cache.TryGetValue(key, out PagedResult<TaskDto>? cached) && cached is not null)
        return cached;

        var paged = await _uow.Tasks.GetPagedAsync(userId, status, from, to, search, page, pageSize);
        var mapped = new PagedResult<TaskDto>
        {
            Items = paged.Items.Select(t => new TaskDto(t.Id, t.Title, t.Description, t.DueDate, t.Status.ToString(), t.AssignedToId, t.AssignedTo?.UserName)).ToList(),
            TotalCount = paged.TotalCount
        };

        _cache.Set(key, mapped, TimeSpan.FromSeconds(30));
        return mapped;
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        if (dto.DueDate <= DateTime.UtcNow) throw new ArgumentException("Due date must be future.");
        var model = new TaskItem { Title = dto.Title, Description = dto.Description, DueDate = dto.DueDate, AssignedToId = dto.AssignedToUserId };
        await _uow.Tasks.AddAsync(model);
        await _uow.SaveChangesAsync();
        _cache.Remove("tasks:*"); // simple approach; better: track keys
        return new TaskDto(model.Id, model.Title, model.Description, model.DueDate, model.Status.ToString(), model.AssignedToId, model.AssignedTo?.UserName);
    }

    public async Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var existing = await _uow.Tasks.GetByIdAsync(id);
        if (existing == null) return null;
        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.DueDate = dto.DueDate;
        if (dto.Status.HasValue) existing.Status = dto.Status.Value;
        existing.AssignedToId = dto.AssignedToUserId;
        await _uow.Tasks.UpdateAsync(existing);
        await _uow.SaveChangesAsync();
        _cache.Remove("tasks:*");
        return new TaskDto(existing.Id, existing.Title, existing.Description, existing.DueDate, existing.Status.ToString(), existing.AssignedToId, existing.AssignedTo?.UserName);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _uow.Tasks.GetByIdAsync(id);
        if (existing == null) return false;
        await _uow.Tasks.DeleteAsync(existing);
        await _uow.SaveChangesAsync();
        _cache.Remove("tasks:*"); // simple invalidation; replace with key tracking for production
        return true;
    }
}

using Application.DTOs;
using Domain.Entities;
using Domain.Enum;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<List<TaskItem>> GetFilteredTasksAsync(int? userId, TaskProgress? status);
    Task<TaskItem> CreateTaskAsync(CreateTaskDto task);

    Task<PagedResult<TaskDto>> GetTasksAsync(int? userId, TaskProgress? status, DateTime? from, DateTime? to, string? search, int page, int pageSize);
    Task<TaskDto> CreateAsync(CreateTaskDto dto);
    Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(int id);
}

    

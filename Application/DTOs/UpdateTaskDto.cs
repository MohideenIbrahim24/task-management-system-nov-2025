using Domain.Enum;

namespace Application.DTOs;

public record UpdateTaskDto(string Title, string? Description, DateTime DueDate, int? AssignedToUserId, TaskProgress? Status);


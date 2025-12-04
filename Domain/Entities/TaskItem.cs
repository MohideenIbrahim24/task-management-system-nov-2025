using Domain.Enum;

namespace Domain.Entities;

public class TaskItem : BaseEntity
{    
    public string Title { get; set; }  = null!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public TaskProgress Status { get; set; } = TaskProgress.Pending; // Pending, InProgress, Completed

    public int? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}

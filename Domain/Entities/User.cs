using System;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string UserPasswordHash { get; set; } = null!;
    public string Role  { get; set; } = "User"; // Admin or User
    public virtual ICollection<TaskItem>? Tasks { get; set; } 
}

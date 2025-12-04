public record TaskDto(int Id, string Title, string? Description, DateTime DueDate, string Status, int? AssignedToUserId, string? AssignedUserName);

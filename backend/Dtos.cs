
public record RegisterDto(string Username, string Password);
public record LoginDto(string Username, string Password);
public record ProjectDto(string Title, string? Description);
public record TaskDto(string Title, DateTime? DueDate);

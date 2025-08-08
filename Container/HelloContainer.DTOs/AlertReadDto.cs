namespace HelloContainer.DTOs;

public record AlertReadDto(Guid Id, Guid ContainerId, string Message, DateTime CreatedAt); 
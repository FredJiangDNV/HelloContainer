namespace HelloContainer.Application.DTOs;

public record DisconnectContainersDto(Guid SourceContainerId, Guid TargetContainerId);

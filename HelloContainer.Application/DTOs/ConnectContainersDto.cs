namespace HelloContainer.Application.DTOs;

public record ConnectContainersDto(Guid SourceContainerId, Guid TargetContainerId);

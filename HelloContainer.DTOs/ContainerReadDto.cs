namespace HelloContainer.DTOs;

public record ContainerReadDto(Guid Id, string Name, double Amount, double Capacity, IEnumerable<Guid> ConnectedContainerIds);

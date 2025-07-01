namespace HelloContainer.Application.DTOs
{
    public class ConnectContainersDto
    {
        public Guid SourceContainerId { get; set; }
        public Guid TargetContainerId { get; set; }
    }
} 
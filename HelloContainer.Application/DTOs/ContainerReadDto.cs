namespace HelloContainer.Application.DTOs
{
    public class ContainerReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public double Capacity { get; set; }
    }
} 
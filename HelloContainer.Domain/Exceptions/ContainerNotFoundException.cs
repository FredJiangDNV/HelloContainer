namespace HelloContainer.Domain.Exceptions
{
    public class ContainerNotFoundException : DomainException
    {
        public ContainerNotFoundException(string message) : base(message)
        {
        }

        public ContainerNotFoundException(Guid containerId) 
            : base($"Container with ID {containerId} was not found.")
        {
        }
    }
} 
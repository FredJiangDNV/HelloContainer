namespace HelloContainer.Domain.Exceptions
{
    public class InvalidConnectionException : DomainException
    {
        public InvalidConnectionException(string message) : base(message)
        {
        }

        public InvalidConnectionException(Guid containerId, Guid targetContainerId, string reason) 
            : base($"Cannot connect container {containerId} to {targetContainerId}. {reason}")
        {
        }
    }
} 
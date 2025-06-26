namespace HelloContainer.Domain.Exceptions
{
    public class ContainerOverflowException : DomainException
    {
        public ContainerOverflowException(string message) : base(message)
        {
        }

        public ContainerOverflowException(double amount, double capacity) 
            : base($"Cannot add {amount} units. Container capacity is {capacity} units.")
        {
        }
    }
} 
using HelloContainer.Domain.Abstractions;

namespace HelloContainer.Domain.AlertAggregate
{
    public class Alert : AggregateRoot
    {
        public Guid ContainerId { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Alert(Guid containerId, string message) : base(Guid.NewGuid())
        {
            ContainerId = containerId;
            Message = message;
            CreatedAt = DateTime.UtcNow;
        }

        public static Alert Create(Guid containerId, string message)
        {
            return new Alert(containerId, message);
        }
    }
}

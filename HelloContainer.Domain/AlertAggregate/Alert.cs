using HelloContainer.Domain.Common;

namespace HelloContainer.Domain.AlertAggregate
{
    public class Alert : Entity, IAggregateRoot
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
            var alert = new Alert(containerId, message);
            return alert;
        }
    }
}

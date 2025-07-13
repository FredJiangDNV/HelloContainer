using HelloContainer.Domain.Common;

namespace HelloContainer.Domain.OutboxAggregate
{
    public class OutboxIntegrationEvent : Entity, IAggregateRoot
    {
        public OutboxIntegrationEvent(string eventName, string eventContent) : base(Guid.NewGuid())
        {
            EventName = eventName;
            EventContent = eventContent;
        }

        public string EventName { get; private set; }
        public string EventContent { get; private set; }

        public static OutboxIntegrationEvent Create(string eventName, string eventContent)
        {
            var outboxEvent = new OutboxIntegrationEvent(eventName, eventContent);
            return outboxEvent;
        }
    }
}

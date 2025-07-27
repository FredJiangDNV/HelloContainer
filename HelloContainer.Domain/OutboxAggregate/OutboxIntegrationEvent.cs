using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.OutboxAggregate
{
    public class OutboxIntegrationEvent : AggregateRoot
    {
        public OutboxIntegrationEvent(string eventName, string eventContent)
        {
            EventName = eventName;
            EventContent = eventContent;
        }

        public string EventName { get; private set; }
        public string EventContent { get; private set; }

        public static OutboxIntegrationEvent Create(string eventName, string eventContent)
        {
            return new OutboxIntegrationEvent(eventName, eventContent);
        }
    }
}

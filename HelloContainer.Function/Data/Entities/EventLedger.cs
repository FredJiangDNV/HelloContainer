using System.Text.Json;

namespace HelloContainer.Function.Data.Entities
{
    public class EventLedger
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string EventData { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public static EventLedger Create(string eventType, object eventData)
        {
            return new EventLedger
            {
                Id = Guid.NewGuid(),
                EventType = eventType,
                EventData = JsonSerializer.Serialize(eventData),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
} 
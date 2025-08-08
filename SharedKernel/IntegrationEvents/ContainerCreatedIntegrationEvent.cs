namespace HelloContainer.SharedKernel.IntegrationEvents
{
    public record ContainerCreatedIntegrationEvent(Guid id, Guid containerId, string name) : IIntegrationEvent
    {
        public string EventType => "ContainerCreated";
    }
}

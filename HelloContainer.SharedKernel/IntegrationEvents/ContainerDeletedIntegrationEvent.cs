namespace HelloContainer.SharedKernel.IntegrationEvents
{
    public record ContainerDeletedIntegrationEvent(Guid id, Guid containerId, string name) : IIntegrationEvent
    {
        public string EventType => "ContainerDeleted";
    }
}

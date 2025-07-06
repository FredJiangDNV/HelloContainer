using HelloContainer.Domain.Common;
using HelloContainer.Domain.ContainerAggregate.Events;
using HelloContainer.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloContainer.Domain.ContainerAggregate
{
    public class Container : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Amount Amount { get; private set; }
        public Capacity Capacity { get; private set; }

        public List<string> ConnectedContainerIdsRaw { get; private set; } = new();

        [NotMapped]
        public IList<Guid> ConnectedContainerIds
        {
            get => ConnectedContainerIdsRaw.Select(Guid.Parse).ToList();
            set => ConnectedContainerIdsRaw = value?.Select(g => g.ToString()).ToList() ?? new();
        }

        private Container(string name, Capacity capacity) : base(Guid.NewGuid())
        {
            Name = name;
            Capacity = capacity;
            Amount = Amount.Create(0);
            ConnectedContainerIds = new List<Guid>();
        }

        public static Container Create(string name, double capacity)
        {
            var container = new Container(name, Capacity.Create(capacity));
            container.Raise(new ContainerCreatedDomainEvent(Guid.NewGuid(), container.Id));
            return container;
        }

        public void ConnectTo(Guid otherContainerId)
        {
            if (otherContainerId == Guid.Empty)
                throw new InvalidConnectionException("Container ID cannot be empty.");

            if (otherContainerId == Id)
                throw new InvalidConnectionException(Id, otherContainerId, "Cannot connect a container to itself.");

            if (ConnectedContainerIds.Contains(otherContainerId))
                throw new InvalidConnectionException(Id, otherContainerId, "Container is already connected.");

            var containers = ConnectedContainerIds.ToList();
            containers.Add(otherContainerId);
            ConnectedContainerIds = containers;
        }

        public void SetWater(double amount)
        {
            Amount = Amount.Create(amount);
        }

        public void Disconnect(Guid otherContainerId)
        {
            if (otherContainerId == Guid.Empty)
                throw new InvalidConnectionException("Container ID cannot be empty.");

            if (!ConnectedContainerIds.Contains(otherContainerId))
                throw new InvalidConnectionException(Id, otherContainerId, "Container is not connected.");

            var containers = ConnectedContainerIds.ToList();
            containers.Remove(otherContainerId);
            ConnectedContainerIds = containers;
        }
    }
}

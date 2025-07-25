using HelloContainer.Domain.ContainerAggregate.Events;
using HelloContainer.Domain.Exceptions;
using HelloContainer.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloContainer.Domain.ContainerAggregate
{
    public class Container : AggregateRoot
    {
        public string Name { get; private set; }
        public Amount Amount { get; private set; }
        public Capacity Capacity { get; private set; }
        public List<string> ConnectedContainerIdsRaw { get; private set; } = new();
        public bool IsDeleted { get; private set; }

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
            container.Raise(new ContainerCreatedDomainEvent(Guid.NewGuid(), container.Id, name));
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

            ConnectedContainerIdsRaw.Add(otherContainerId.ToString());
        }

        public void SetWater(double amount)
        {
            if (amount > Capacity.Value)
                throw new ContainerOverflowException(amount, Capacity.Value);

            Amount = Amount.Create(amount);

            if (amount >= Capacity.Value * 0.8)
            {
                this.Raise(new WaterOverflowedDomainEvent(Guid.NewGuid(), Id, Name));
            }
        }

        public void Disconnect(Guid otherContainerId)
        {
            if (otherContainerId == Guid.Empty)
                throw new InvalidConnectionException("Container ID cannot be empty.");

            if (!ConnectedContainerIds.Contains(otherContainerId))
                throw new InvalidConnectionException(Id, otherContainerId, "Container is not connected.");

            ConnectedContainerIdsRaw.Remove(otherContainerId.ToString());
        }

        public void Delete()
        {
            IsDeleted = true;
            this.Raise(new ContainerDeletedDomainEvent(Guid.NewGuid(), Id, Name));
        }
    }
}

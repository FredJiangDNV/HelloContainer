using HelloContainer.Domain.Events;
using HelloContainer.Domain.Primitives;
using HelloContainer.Domain.Exceptions;

namespace HelloContainer.Domain
{
    public class Container : Entity, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public Amount Amount { get; private set; }
        public Capacity Capacity { get; private set; }
        public List<Container> ConnectedContainers { get; private set; }

        private Container(string name, Capacity capacity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Capacity = capacity;
            Amount = Amount.Create(0);
            ConnectedContainers = new List<Container>();
        }

        public static Container Create(string name, double capacity)
        {
            var containerCapacity = Capacity.Create(capacity);
            var container = new Container(name, containerCapacity);
            container.Raise(new ContainerCreatedDomainEvent(Guid.NewGuid(), container.Id));
            return container;
        }

        public void ConnectTo(Container other)
        {
            if (other == null)
            {
                throw new InvalidConnectionException("Container cannot be null.");
            }

            if (other.Id == Id)
            {
                throw new InvalidConnectionException(Id, other.Id, "Cannot connect a container to itself.");
            }

            if (ConnectedContainers.Contains(other))
            {
                throw new InvalidConnectionException(Id, other.Id, "Container is already connected.");
            }

            double newAmount = CalculateNewAmount(other);

            ConnectedContainers.Add(other);
            other.ConnectedContainers.Add(this);

            var allContainers = ConnectedContainers.Concat(other.ConnectedContainers).Distinct().ToList();
            foreach (var c in allContainers)
            {
                c.SetAmount(Amount.Create(newAmount));
            }
        }

        private double CalculateNewAmount(Container other)
        {
            var currentGroupSize = ConnectedContainers.Count() + 1;
            var otherGroupSize = other.ConnectedContainers.Count() + 1;

            double currentGroupTotalAmount = currentGroupSize * Amount.Value;
            double otherGroupTotalAmount = otherGroupSize * other.Amount.Value;

            return (currentGroupTotalAmount + otherGroupTotalAmount) / (currentGroupSize + otherGroupSize);
        }

        public double GetAmount()
        {
            return Amount.Value;
        }

        public void SetAmount(Amount newAmount)
        {
            if (newAmount.Value > Capacity.Value)
            {
                throw new ContainerOverflowException(newAmount.Value, Capacity.Value);
            }
            
            Amount = newAmount;
        }

        public void AddAndDistributeWater(double amount)
        {
            var amountPerContainer = amount / (ConnectedContainers.Count + 1);
            var newAmount = Amount.Value + amountPerContainer;
            
            if (newAmount > Capacity.Value)
            {
                throw new ContainerOverflowException(amount, Capacity.Value);
            }
            
            Amount = Amount.Create(newAmount);
            foreach (var container in ConnectedContainers)
            {
                container.AddWater(amountPerContainer);
            }
        }

        public void AddWater(double amount)
        {
            var newAmount = Amount.Value + amount;
            
            if (newAmount > Capacity.Value)
            {
                throw new ContainerOverflowException(amount, Capacity.Value);
            }
            
            Amount = Amount.Create(newAmount);
        }
    }
}

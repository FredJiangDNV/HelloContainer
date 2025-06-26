using HelloContainer.Domain.Events;
using HelloContainer.Domain.Primitives;

namespace HelloContainer.Domain
{
    public class Container : Entity
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
                throw new ArgumentNullException(nameof(other), "Container cannot be null.");
            }

            if (other.Id == Id)
            {
                throw new InvalidOperationException("Cannot connect a container to itself.");
            }

            if (ConnectedContainers.Contains(other))
            {
                throw new InvalidOperationException("Container is already connected.");
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

            double currentGroupTotalAmount = currentGroupSize * Amount;
            double otherGroupTotalAmount = otherGroupSize * other.Amount;

            return (currentGroupTotalAmount + otherGroupTotalAmount) / (currentGroupSize + otherGroupSize);
        }

        public double GetAmount()
        {
            return Amount;
        }

        public void SetAmount(Amount newAmount)
        {
            Amount = newAmount;
        }

        public void AddAndDistributeWater(double amount)
        {
            var amountPerContainer = amount / (ConnectedContainers.Count + 1);
            Amount = Amount.Create(Amount + amountPerContainer);
            foreach (var container in ConnectedContainers)
            {
                container.AddWater(amountPerContainer);
            }
        }

        public void AddWater(double amount)
        {
            Amount = Amount.Create(Amount + amount);
        }

        public bool IsOverflowing(double thresholdPercentage = 0.8)
        {
            var threshold = Capacity * thresholdPercentage;
            return Amount >= threshold;
        }
    }
}

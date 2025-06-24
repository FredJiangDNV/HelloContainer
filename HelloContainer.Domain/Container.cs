using HelloContainer.Domain.Events;
using HelloContainer.Domain.Primitives;

namespace HelloContainer.Domain
{
    public class Container : Entity
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public double Amount { get; private set; }
        public double Capacity { get; private set; }
        public List<Container> ConnectedContainers { get; private set; }

        private Container(string name, double capacity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Capacity = capacity;
            ConnectedContainers = new List<Container>();
        }

        public static Container Create(string name, double capacity)
        {
            var container = new Container(name, capacity);
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
                c.SetAmount(newAmount);
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

        public void SetAmount(double newAmount)
        {
            Amount = newAmount;
        }

        public void AddAndDistributeWater(double amount)
        {
            var amountPerContainer = amount / (ConnectedContainers.Count + 1);
            Amount += amountPerContainer;
            foreach (var container in ConnectedContainers)
            {
                container.AddWater(amountPerContainer);
            }
        }

        public void AddWater(double amount)
        {
            Amount += amount;
        }
    }
}

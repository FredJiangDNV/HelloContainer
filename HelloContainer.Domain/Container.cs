namespace HelloContainer.Domain
{
    public class Container
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
            return new Container(name, capacity);
        }

        public double GetAmount()
        {
            return Amount;
        }

        public void ConnectTo(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container), "Container cannot be null.");
            }

            if (container.Id == Id)
            {
                throw new InvalidOperationException("Cannot connect a container to itself.");
            }

            if (ConnectedContainers.Contains(container))
            {
                throw new InvalidOperationException("Container is already connected.");
            }

            // TODO: Code need improve
            var currentGroupSize = ConnectedContainers.Count() + 1;
            var otherGroupSize = container.ConnectedContainers.Count() + 1;

            double currentGroupTotalAmount = currentGroupSize * Amount;
            double otherGroupTotalAmount = otherGroupSize * container.Amount;

            double newAmount = (currentGroupTotalAmount + otherGroupTotalAmount) / (currentGroupSize + otherGroupSize);

            ConnectedContainers.Add(container);
            container.ConnectedContainers.Add(this);

            var allConnectedContainers = ConnectedContainers.Concat(container.ConnectedContainers).Distinct().ToList();

            foreach (var item in allConnectedContainers)
            {
                item.Amount = newAmount;
            }
        }

        public void AddWater(double amount)
        {
            Amount += amount;
        }
    }
}

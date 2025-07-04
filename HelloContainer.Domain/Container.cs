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
        public IList<Container> ConnectedContainers { get; private set; }

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
            var container = new Container(name, Capacity.Create(capacity));
            container.Raise(new ContainerCreatedDomainEvent(Guid.NewGuid(), container.Id));
            return container;
        }

        public void ConnectTo(Container other)
        {
            if (other == null)
                throw new InvalidConnectionException("Container cannot be null.");

            if (other.Id == Id)
                throw new InvalidConnectionException(Id, other.Id, "Cannot connect a container to itself.");

            if (ConnectedContainers.Contains(other))
                throw new InvalidConnectionException(Id, other.Id, "Container is already connected.");

            ConnectedContainers.Add(other);
            other.ConnectedContainers.Add(this);

            var all = GetAllConnectedContainers();
            double total = all.Sum(c => c.Amount.Value);
            double avg = total / all.Count;
            foreach (var c in all)
                c.Amount = Amount.Create(avg);
        }

        public void AddWater(double amount)
        {
            var all = GetAllConnectedContainers();
            double total = all.Sum(c => c.Amount.Value) + amount;
            double avg = total / all.Count;
            foreach (var c in all)
                c.Amount = Amount.Create(avg);
        }

        private List<Container> GetAllConnectedContainers()
        {
            var visited = new HashSet<Container>();
            var queue = new Queue<Container>();
            queue.Enqueue(this);
            visited.Add(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var next in current.ConnectedContainers)
                {
                    if (!visited.Contains(next))
                    {
                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }
            return visited.ToList();
        }
    }
}

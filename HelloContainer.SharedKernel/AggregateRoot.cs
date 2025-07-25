namespace HelloContainer.SharedKernel
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot(Guid id) : base(id)
        {
        }

        private readonly List<DomainEvent> _domainEvents = new();

        public ICollection<DomainEvent> DomainEvents => _domainEvents;

        public List<DomainEvent> PopDomainEvents()
        {
            var copy = _domainEvents.ToList();
            _domainEvents.Clear();

            return copy;
        }

        protected void Raise(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}

namespace HelloContainer.Domain.Primitives
{
    public abstract class Entity
    {
        private readonly List<DomainEvent> _domainEvents = new ();

        public ICollection<DomainEvent> DomainEvents => _domainEvents;

        public IReadOnlyList<DomainEvent> GetDomainEvents()
        {
            return _domainEvents.ToList();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void Raise(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}

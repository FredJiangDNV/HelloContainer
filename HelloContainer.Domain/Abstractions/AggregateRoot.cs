using HelloContainer.Domain.Common;

namespace HelloContainer.Domain.Abstractions
{
    public abstract class AggregateRoot : Entity
    {
        protected AggregateRoot(Guid id) : base(id)
        {
        }

        private readonly List<DomainEvent> _domainEvents = new();

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

using MediatR;
namespace HelloContainer.SharedKernel;

public record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}

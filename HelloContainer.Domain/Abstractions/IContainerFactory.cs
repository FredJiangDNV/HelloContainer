using HelloContainer.Domain.ContainerAggregate;

namespace HelloContainer.Domain.Abstractions;

public interface IContainerFactory
{
    Task<Container> CreateContainer(string name, double capacity);
}

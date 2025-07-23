using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.ContainerAggregate;

namespace HelloContainer.Domain.Services;

public class ContainerFactory
{
    private readonly IContainerRepository _containerRepository;

    public ContainerFactory(IContainerRepository containerRepository)
    {
        _containerRepository = containerRepository;
    }

    public async Task<Container> CreateContainer(string name, double capacity)
    {
        var exists = (await _containerRepository.FindAsync(x => x.Name == name)).Any();
        if (exists)
            throw new Exception($"Container name '{name}' already exists.");

        var container = Container.Create(name, capacity);
        _containerRepository.Add(container);
        return container;
    }
}

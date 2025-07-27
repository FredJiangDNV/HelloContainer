using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.ContainerAggregate;
using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.Services;

public class ContainerFactory
{
    private readonly IContainerRepository _containerRepository;

    public ContainerFactory(IContainerRepository containerRepository)
    {
        _containerRepository = containerRepository;
    }

    public async Task<Result<Container>> CreateContainer(string name, double capacity)
    {
        var exists = (await _containerRepository.FindAsync(x => x.Name == name)).Any();
        if (exists)
            return Result.Failure<Container>(Error.Conflict("Container.NameExists", $"Container name '{name}' already exists."));

        var container = Container.Create(name, capacity);
        if (container.IsFailure)
            return container;

        _containerRepository.Add(container.Value);
        return container;
    }
}

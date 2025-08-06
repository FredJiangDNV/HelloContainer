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
            return Result.Failure<Container>(ContainerErrors.Conflict.NameExists(name));

        return Container.Create(name, capacity);
    }
}

using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.ContainerAggregate;
using HelloContainer.Domain.Exceptions;

namespace HelloContainer.Domain.Services
{
    public class ContainerManager
    {
        private readonly IContainerRepository _repository;

        public ContainerManager(IContainerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Container?> AddWater(Guid containerId, double amount)
        {
            var container = await GetContainerOrThrow(containerId);
            await SetWaterForAllConnectedContainers(containerId, amount);
            return container;
        }

        public async Task<Container> ConnectContainers(Guid sourceId, Guid targetId)
        {
            var source = await GetContainerOrThrow(sourceId);
            var target = await GetContainerOrThrow(targetId);

            source!.ConnectTo(targetId);
            target!.ConnectTo(sourceId);

            await SetWaterForAllConnectedContainers(sourceId);
            return source;
        }

        public async Task<Container> DisconnectContainers(Guid sourceId, Guid targetId)
        {
            var source = await GetContainerOrThrow(sourceId);
            var target = await GetContainerOrThrow(targetId);

            source!.Disconnect(targetId);
            target!.Disconnect(sourceId);

            return source;
        }

        private async Task SetWaterForAllConnectedContainers(Guid containerId, double addedAmount = 0)
        {
            var allConnectedContainers = await GetAllConnectedContainers(containerId);
            double total = allConnectedContainers.Sum(c => c.Amount.Value) + addedAmount;
            double avg = Math.Round(total / allConnectedContainers.Count, 2);

            foreach (var c in allConnectedContainers)
                c.SetWater(avg);
        }

        private async Task<Container?> GetContainerOrThrow(Guid sourceId)
        {
            var source = await _repository.GetById(sourceId);
            if (source == null)
                throw new ContainerNotFoundException(sourceId);
            return source;
        }

        private async Task<List<Container>> GetAllConnectedContainers(Guid containerId)
        {
            var visited = new HashSet<Guid>();
            var queue = new Queue<Guid>();
            queue.Enqueue(containerId);
            visited.Add(containerId);

            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                var currentContainer = await _repository.GetById(currentId);
                
                if (currentContainer == null)
                    continue;

                foreach (var connectedId in currentContainer.ConnectedContainerIds)
                {
                    if (!visited.Contains(connectedId))
                    {
                        visited.Add(connectedId);
                        queue.Enqueue(connectedId);
                    }
                }
            }

            var result = new List<Container>();
            foreach (var id in visited)
            {
                var container = await _repository.GetById(id);
                if (container != null)
                {
                    result.Add(container);
                }
            }

            return result;
        }
    }
}

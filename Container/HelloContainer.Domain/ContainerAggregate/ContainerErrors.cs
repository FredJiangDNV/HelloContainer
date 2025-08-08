using HelloContainer.SharedKernel;

namespace HelloContainer.Domain.ContainerAggregate;

public static class ContainerErrors
{
    public static class Validation
    {
        public static Error EmptyName => Error.Validation(
            "Container.InvalidName",
            "Container name cannot be empty.");

        public static Error InvalidCapacity(double capacity) => Error.Validation(
            "Container.InvalidCapacity",
            $"Container capacity must be greater than zero. Current value: {capacity}");
    }

    public static class Conflict
    {
        public static Error NameExists(string name) => Error.Conflict(
            "Container.NameExists",
            $"Container name '{name}' already exists.");
    }
} 
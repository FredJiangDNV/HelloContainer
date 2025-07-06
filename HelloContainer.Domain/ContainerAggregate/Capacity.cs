namespace HelloContainer.Domain.ContainerAggregate
{
    public record Capacity(double Value)
    {
        public static Capacity Create(double value)
        {
            if (value <= 0)
                throw new ArgumentException("Capacity must be greater than zero.", nameof(value));

            return new Capacity(value);
        }

        public static implicit operator double(Capacity capacity) => capacity.Value;
    }
}
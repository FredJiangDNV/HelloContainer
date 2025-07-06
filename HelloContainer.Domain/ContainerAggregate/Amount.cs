namespace HelloContainer.Domain.ContainerAggregate
{
    public record Amount(double Value)
    {
        public static Amount Create(double value)
        {
            if (value < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(value));

            return new Amount(value);
        }

        public static implicit operator double(Amount amount) => amount.Value;
    }
}
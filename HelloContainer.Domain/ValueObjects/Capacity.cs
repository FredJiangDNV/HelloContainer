using HelloContainer.Domain.Primitives;

namespace HelloContainer.Domain.ValueObjects
{
    public class Capacity : ValueObject
    {
        public double Value { get; }

        private Capacity(double value)
        {
            if (value <= 0)
                throw new ArgumentException("Capacity must be greater than zero.", nameof(value));
            
            Value = value;
        }

        public static Capacity Create(double value)
        {
            return new Capacity(value);
        }

        public static implicit operator double(Capacity capacity) => capacity.Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
} 
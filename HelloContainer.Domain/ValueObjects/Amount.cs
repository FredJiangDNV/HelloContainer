using HelloContainer.Domain.Primitives;

namespace HelloContainer.Domain.ValueObjects
{
    public class Amount : ValueObject
    {
        public double Value { get; }

        private Amount(double value)
        {
            if (value < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(value));
            
            Value = value;
        }

        public static Amount Create(double value)
        {
            return new Amount(value);
        }

        public static implicit operator double(Amount amount) => amount.Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
} 
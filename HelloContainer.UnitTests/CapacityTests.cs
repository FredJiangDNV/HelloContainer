using HelloContainer.Domain;
using Xunit;

namespace HelloContainer.UnitTests
{
    public class CapacityTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldCreateCapacity()
        {
            // Arrange
            double validValue = 100.0;

            // Act
            var capacity = Capacity.Create(validValue);

            // Assert
            Assert.NotNull(capacity);
            Assert.Equal(validValue, capacity);
        }

        [Fact]
        public void Create_WithZeroValue_ShouldThrowArgumentException()
        {
            // Arrange
            double zeroValue = 0.0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Capacity.Create(zeroValue));
            Assert.Contains("Capacity must be greater than zero", exception.Message);
        }

        [Fact]
        public void Create_WithNegativeValue_ShouldThrowArgumentException()
        {
            // Arrange
            double negativeValue = -10.0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Capacity.Create(negativeValue));
            Assert.Contains("Capacity must be greater than zero", exception.Message);
        }

        [Fact]
        public void ImplicitConversion_ToDouble_ShouldReturnValue()
        {
            // Arrange
            var capacity = Capacity.Create(150.5);

            // Act
            double result = capacity;

            // Assert
            Assert.Equal(150.5, result);
        }

        [Fact]
        public void Equals_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var capacity1 = Capacity.Create(100.0);
            var capacity2 = Capacity.Create(100.0);

            // Act & Assert
            Assert.Equal(capacity1, capacity2);
            Assert.True(capacity1.Equals(capacity2));
        }

        [Fact]
        public void Equals_DifferentValues_ShouldReturnFalse()
        {
            // Arrange
            var capacity1 = Capacity.Create(100.0);
            var capacity2 = Capacity.Create(200.0);

            // Act & Assert
            Assert.NotEqual(capacity1, capacity2);
            Assert.False(capacity1.Equals(capacity2));
        }
    }
}
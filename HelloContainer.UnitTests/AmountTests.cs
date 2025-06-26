using HelloContainer.Domain.ValueObjects;
using Xunit;

namespace HelloContainer.UnitTests
{
    public class AmountTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldCreateAmount()
        {
            // Arrange
            double validValue = 50.0;

            // Act
            var amount = Amount.Create(validValue);

            // Assert
            Assert.NotNull(amount);
            Assert.Equal(validValue, amount.Value);
        }

        [Fact]
        public void Create_WithZeroValue_ShouldCreateAmount()
        {
            // Arrange
            double zeroValue = 0.0;

            // Act
            var amount = Amount.Create(zeroValue);

            // Assert
            Assert.NotNull(amount);
            Assert.Equal(zeroValue, amount.Value);
        }

        [Fact]
        public void Create_WithNegativeValue_ShouldThrowArgumentException()
        {
            // Arrange
            double negativeValue = -10.0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Amount.Create(negativeValue));
            Assert.Contains("Amount cannot be negative", exception.Message);
        }

        [Fact]
        public void ImplicitConversion_ToDouble_ShouldReturnValue()
        {
            // Arrange
            var amount = Amount.Create(75.5);

            // Act
            double result = amount;

            // Assert
            Assert.Equal(75.5, result);
        }

        [Fact]
        public void Equals_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var amount1 = Amount.Create(100.0);
            var amount2 = Amount.Create(100.0);

            // Act & Assert
            Assert.Equal(amount1, amount2);
            Assert.True(amount1.Equals(amount2));
        }

        [Fact]
        public void Equals_DifferentValues_ShouldReturnFalse()
        {
            // Arrange
            var amount1 = Amount.Create(100.0);
            var amount2 = Amount.Create(200.0);

            // Act & Assert
            Assert.NotEqual(amount1, amount2);
            Assert.False(amount1.Equals(amount2));
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var amount = Amount.Create(100.0);

            // Act & Assert
            Assert.False(amount.Equals(null));
        }

        [Fact]
        public void Equals_WithDifferentType_ShouldReturnFalse()
        {
            // Arrange
            var amount = Amount.Create(100.0);
            var capacity = Capacity.Create(100.0);

            // Act & Assert
            Assert.False(amount.Equals(capacity));
        }

        [Fact]
        public void OperatorEquality_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var amount1 = Amount.Create(100.0);
            var amount2 = Amount.Create(100.0);

            // Act & Assert
            Assert.True(amount1 == amount2);
        }

        [Fact]
        public void OperatorInequality_DifferentValues_ShouldReturnTrue()
        {
            // Arrange
            var amount1 = Amount.Create(100.0);
            var amount2 = Amount.Create(200.0);

            // Act & Assert
            Assert.True(amount1 != amount2);
        }
    }
}
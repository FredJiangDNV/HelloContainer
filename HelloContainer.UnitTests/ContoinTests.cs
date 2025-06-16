using Xunit.Abstractions;
using Xunit;
using HelloContainer.Domain;

namespace HelloContainer.UnitTests
{
    public class ContoinTests
    {
        private readonly ITestOutputHelper outputHelper;

        public ContoinTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void ConnectTo_WhenTwoContainersConnected_ShouldShareWaterEqually()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);
            var c = Container.Create("c", 100);
            var d = Container.Create("d", 100);

            // Act
            a.AddWater(12);
            d.AddWater(8);
            a.ConnectTo(b);

            // Assert
            outputHelper.WriteLine($"Container A: {a.GetAmount()}");
            outputHelper.WriteLine($"Container B: {b.GetAmount()}");
            outputHelper.WriteLine($"Container C: {c.GetAmount()}");
            outputHelper.WriteLine($"Container D: {d.GetAmount()}");

            Assert.Equal(6, a.GetAmount());
            Assert.Equal(6, b.GetAmount());
            Assert.Equal(0, c.GetAmount());
            Assert.Equal(8, d.GetAmount());
        }

        [Fact]
        public void Create_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var container = Container.Create("TestContainer", 100);

            // Assert
            Assert.NotEqual(Guid.Empty, container.Id);
            Assert.Equal("TestContainer", container.Name);
            Assert.Equal(100, container.Capacity);
            Assert.Empty(container.ConnectedContainers);
        }

        [Fact]
        public void AddWater_ShouldIncreaseAmount()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act
            container.AddWater(10);

            // Assert
            Assert.Equal(10, container.GetAmount());
        }

        [Fact]
        public void SetAmount_ShouldUpdateAmount()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act
            container.SetAmount(20);

            // Assert
            Assert.Equal(20, container.GetAmount());
        }

        [Fact]
        public void ConnectTo_NullContainer_ShouldThrowArgumentNullException()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => container.ConnectTo(null));
        }

        [Fact]
        public void ConnectTo_Self_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => container.ConnectTo(container));
        }

        [Fact]
        public void ConnectTo_AlreadyConnected_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var containerA = Container.Create("A", 100);
            var containerB = Container.Create("B", 100);
            containerA.ConnectTo(containerB);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => containerA.ConnectTo(containerB));
        }

        [Fact]
        public void ConnectTo_ShouldShareWaterEqually()
        {
            // Arrange
            var containerA = Container.Create("A", 100);
            var containerB = Container.Create("B", 100);
            containerA.AddWater(10);
            containerB.AddWater(20);

            // Act
            containerA.ConnectTo(containerB);

            // Assert
            Assert.Equal(15, containerA.GetAmount());
            Assert.Equal(15, containerB.GetAmount());
        }
    }
}

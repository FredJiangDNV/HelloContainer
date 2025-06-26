using Xunit;
using HelloContainer.Domain;

namespace HelloContainer.UnitTests
{
    public class ContoinTests
    {
        [Fact]
        public void ConnectTo_WhenTwoContainersConnected_ShouldShareWaterEqually()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);
            var c = Container.Create("c", 100);
            var d = Container.Create("d", 100);

            // Act
            a.AddAndDistributeWater(12);
            d.AddAndDistributeWater(8);
            a.ConnectTo(b);

            // Assert
            Assert.Equal(6, a.GetAmount());
            Assert.Equal(6, b.GetAmount());
            Assert.Equal(0, c.GetAmount());
            Assert.Equal(8, d.GetAmount());
        }

        [Fact]
        public void AddWater_WhenContainersNotConnect()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);
            var c = Container.Create("c", 100);
            var d = Container.Create("d", 100);

            // Act
            a.AddAndDistributeWater(12);
            d.AddAndDistributeWater(8);

            // Assert
            Assert.Equal(12, a.GetAmount());
            Assert.Equal(0, b.GetAmount());
            Assert.Equal(0, c.GetAmount());
            Assert.Equal(8, d.GetAmount());
        }

        [Fact]
        public void AddWater_WhenTwoContainersConnected_ShouldShareWaterEqually()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);
            var c = Container.Create("c", 100);
            var d = Container.Create("d", 100);

            // Act
            a.ConnectTo(b);
            a.AddAndDistributeWater(12);
            d.AddAndDistributeWater(8);

            // Assert
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
            Assert.Equal(100, container.Capacity.Value);
            Assert.Empty(container.ConnectedContainers);
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
    }
}

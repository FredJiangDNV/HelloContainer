using Xunit;
using HelloContainer.Domain.ContainerAggregate;
using HelloContainer.Domain.Exceptions;

namespace HelloContainer.UnitTests
{
    public class ContainerTests
    {
        [Fact]
        public void ConnectTo_EmptyGuid_ShouldThrowInvalidConnectionException()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => container.ConnectTo(Guid.Empty));
        }

        [Fact]
        public void ConnectTo_Self_ShouldThrowInvalidConnectionException()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => container.ConnectTo(container.Id));
        }

        [Fact]
        public void ConnectTo_AlreadyConnected_ShouldThrowInvalidConnectionException()
        {
            // Arrange
            var containerA = Container.Create("A", 100);
            var containerB = Container.Create("B", 100);
            containerA.ConnectTo(containerB.Id);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => containerA.ConnectTo(containerB.Id));
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
            Assert.Empty(container.ConnectedContainerIds);
        }

        [Fact]
        public void ConnectTo_WhenTwoContainersConnected_ShouldAddConnection()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);

            // Act
            a.ConnectTo(b.Id);

            // Assert
            Assert.Contains(b.Id, a.ConnectedContainerIds);
        }

        [Fact]
        public void Disconnect_WhenContainersConnected_ShouldRemoveConnection()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);
            a.ConnectTo(b.Id);

            // Act
            a.Disconnect(b.Id);

            // Assert
            Assert.DoesNotContain(b.Id, a.ConnectedContainerIds);
        }

        [Fact]
        public void Disconnect_WhenNotConnected_ShouldThrowInvalidConnectionException()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => a.Disconnect(b.Id));
        }
    }
}

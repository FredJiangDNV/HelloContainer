using Xunit;
using HelloContainer.Domain.ContainerAggregate;
using HelloContainer.Domain.Exceptions;
using HelloContainer.SharedKernel;

namespace HelloContainer.UnitTests
{
    public class ContainerTests
    {
        [Fact]
        public void Create_WithValidInput_ShouldSucceed()
        {
            // Act
            var result = Container.Create("TestContainer", 100);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value.Id);
            Assert.Equal("TestContainer", result.Value.Name);
            Assert.Equal(100, result.Value.Capacity.Value);
            Assert.Empty(result.Value.ConnectedContainerIds);
        }

        [Fact]
        public void Create_WithEmptyName_ShouldFail()
        {
            // Act
            var result = Container.Create("", 100);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Validation.Container.InvalidName", result.Error.Code);
        }

        [Fact]
        public void Create_WithZeroCapacity_ShouldFail()
        {
            // Act
            var result = Container.Create("TestContainer", 0);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Validation.Container.InvalidCapacity", result.Error.Code);
        }

        [Fact]
        public void Create_WithNegativeCapacity_ShouldFail()
        {
            // Act
            var result = Container.Create("TestContainer", -10);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Validation.Container.InvalidCapacity", result.Error.Code);
        }

        [Fact]
        public void ConnectTo_EmptyGuid_ShouldThrowInvalidConnectionException()
        {
            // Arrange
            var containerResult = Container.Create("TestContainer", 100);
            Assert.True(containerResult.IsSuccess);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => containerResult.Value.ConnectTo(Guid.Empty));
        }

        [Fact]
        public void ConnectTo_Self_ShouldThrowInvalidConnectionException()
        {
            // Arrange
            var containerResult = Container.Create("TestContainer", 100);
            Assert.True(containerResult.IsSuccess);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => containerResult.Value.ConnectTo(containerResult.Value.Id));
        }

        [Fact]
        public void ConnectTo_AlreadyConnected_ShouldThrowInvalidConnectionException()
        {
            // Arrange
            var containerAResult = Container.Create("A", 100);
            var containerBResult = Container.Create("B", 100);
            Assert.True(containerAResult.IsSuccess);
            Assert.True(containerBResult.IsSuccess);

            var containerA = containerAResult.Value;
            var containerB = containerBResult.Value;
            containerA.ConnectTo(containerB.Id);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => containerA.ConnectTo(containerB.Id));
        }

        [Fact]
        public void ConnectTo_WhenTwoContainersConnected_ShouldAddConnection()
        {
            // Arrange
            var aResult = Container.Create("a", 100);
            var bResult = Container.Create("b", 100);

            var a = aResult.Value;
            var b = bResult.Value;

            // Act
            a.ConnectTo(b.Id);

            // Assert
            Assert.Contains(b.Id, a.ConnectedContainerIds);
        }

        [Fact]
        public void Disconnect_WhenContainersConnected_ShouldRemoveConnection()
        {
            // Arrange
            var aResult = Container.Create("a", 100);
            var bResult = Container.Create("b", 100);

            var a = aResult.Value;
            var b = bResult.Value;

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
            var aResult = Container.Create("a", 100);
            var bResult = Container.Create("b", 100);
            var a = aResult.Value;
            var b = bResult.Value;

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => a.Disconnect(b.Id));
        }
    }
}

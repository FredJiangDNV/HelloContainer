using Xunit;
using HelloContainer.Domain;
using HelloContainer.Domain.Exceptions;

namespace HelloContainer.UnitTests
{
    public class ContoinTests
    {
        [Fact]
        public void ConnectTo_NullContainer_ShouldThrowArgumentNullException()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => container.ConnectTo(null));
        }

        [Fact]
        public void ConnectTo_Self_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var container = Container.Create("TestContainer", 100);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => container.ConnectTo(container));
        }

        [Fact]
        public void ConnectTo_AlreadyConnected_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var containerA = Container.Create("A", 100);
            var containerB = Container.Create("B", 100);
            containerA.ConnectTo(containerB);

            // Act & Assert
            Assert.Throws<InvalidConnectionException>(() => containerA.ConnectTo(containerB));
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
            Assert.Equal(6, a.Amount.Value);
            Assert.Equal(6, b.Amount.Value);
            Assert.Equal(0, c.Amount.Value);
            Assert.Equal(8, d.Amount.Value);
        }

        [Fact]
        public void ComplexConnectionAndWaterDistribution_ShouldShareWaterEquallyAmongAllConnected()
        {
            // Arrange
            var a = Container.Create("a", 100);
            var b = Container.Create("b", 100);
            var c = Container.Create("c", 100);
            var d = Container.Create("d", 100);
            var e = Container.Create("e", 100);

            // Act
            a.AddWater(10); // a:10, b:0, c:0, d:0, e:0
            AssertAmount(10, 0, 0, 0, 0);

            b.AddWater(20); // a:10, b:20, c:0, d:0, e:0
            AssertAmount(10, 20, 0, 0, 0);

            c.AddWater(30); // a:10, b:20, c:30, d:0, e:0
            AssertAmount(10, 20, 30, 0, 0);

            a.ConnectTo(b); // a:15, b:15, c:30, d:0, e:0
            AssertAmount(15, 15, 30, 0, 0);

            b.ConnectTo(c); // a:20, b:20, c:20, d:0, e:0
            AssertAmount(20, 20, 20, 0, 0);

            d.AddWater(40); // a:20, b:20, c:20, d:40, e:0
            AssertAmount(20, 20, 20, 40, 0);

            d.ConnectTo(e); // a:20, b:20, c:20, d:20, e:20
            AssertAmount(20, 20, 20, 20, 20);

            c.ConnectTo(d); // a:20, b:20, c:20, d:20, e:20
            AssertAmount(20, 20, 20, 20, 20);

            a.AddWater(5); // a:21, b:21, c:21, d:21, e:21
            AssertAmount(21, 21, 21, 21, 21);

            void AssertAmount(double aAmount, double bAmount, double cAmount, double dAmount, double eAmount)
            {
                Assert.Equal(aAmount, a.Amount.Value);
                Assert.Equal(bAmount, b.Amount.Value);
                Assert.Equal(cAmount, c.Amount.Value);
                Assert.Equal(dAmount, d.Amount.Value);
                Assert.Equal(eAmount, e.Amount.Value);
            }
        }
    }
}

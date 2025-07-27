using Xunit;
using HelloContainer.Domain.ContainerAggregate;
using HelloContainer.Domain.Services;
using HelloContainer.Domain.Abstractions;
using Moq;

namespace HelloContainer.UnitTests
{
    public class ContainerManagerTests
    {
        private readonly Mock<IContainerRepository> _mockRepository;
        private readonly ContainerManager _containerManager;

        public ContainerManagerTests()
        {
            _mockRepository = new Mock<IContainerRepository>();
            _containerManager = new ContainerManager(_mockRepository.Object);
        }

        [Fact]
        public async Task ComplexConnectionAndWaterDistribution_ShouldShareWaterEquallyAmongAllConnected()
        {
            // Arrange
            var aResult = Container.Create("a", 100);
            var bResult = Container.Create("b", 100);
            var cResult = Container.Create("c", 100);
            var dResult = Container.Create("d", 100);
            var eResult = Container.Create("e", 100);

            Assert.True(aResult.IsSuccess);
            Assert.True(bResult.IsSuccess);
            Assert.True(cResult.IsSuccess);
            Assert.True(dResult.IsSuccess);
            Assert.True(eResult.IsSuccess);

            var a = aResult.Value;
            var b = bResult.Value;
            var c = cResult.Value;
            var d = dResult.Value;
            var e = eResult.Value;

            // Setup repository to return containers by ID
            _mockRepository.Setup(r => r.GetById(a.Id)).ReturnsAsync(a);
            _mockRepository.Setup(r => r.GetById(b.Id)).ReturnsAsync(b);
            _mockRepository.Setup(r => r.GetById(c.Id)).ReturnsAsync(c);
            _mockRepository.Setup(r => r.GetById(d.Id)).ReturnsAsync(d);
            _mockRepository.Setup(r => r.GetById(e.Id)).ReturnsAsync(e);

            // Act & Assert
            // Initial state
            await _containerManager.AddWater(a.Id, 10);
            AssertAmount(10, 0, 0, 0, 0);

            await _containerManager.AddWater(b.Id, 20);
            AssertAmount(10, 20, 0, 0, 0);

            await _containerManager.AddWater(c.Id, 30);
            AssertAmount(10, 20, 30, 0, 0);

            // Connect a and b
            await _containerManager.ConnectContainers(a.Id, b.Id);
            AssertAmount(15, 15, 30, 0, 0);

            // Connect b and c
            await _containerManager.ConnectContainers(b.Id, c.Id);
            AssertAmount(20, 20, 20, 0, 0);

            // Add water to d
            await _containerManager.AddWater(d.Id, 40);
            AssertAmount(20, 20, 20, 40, 0);

            // Connect d and e
            await _containerManager.ConnectContainers(d.Id, e.Id);
            AssertAmount(20, 20, 20, 20, 20);

            // Connect c and d
            await _containerManager.ConnectContainers(c.Id, d.Id);
            AssertAmount(20, 20, 20, 20, 20);

            // Add water to a
            await _containerManager.AddWater(a.Id, 5);
            AssertAmount(21, 21, 21, 21, 21);

            // Remove water from a
            await _containerManager.AddWater(a.Id, -10);
            AssertAmount(19, 19, 19, 19, 19);

            // Disconnect d and e
            await _containerManager.DisconnectContainers(d.Id, e.Id);
            AssertAmount(19, 19, 19, 19, 19);

            // Add water to a after disconnection
            await _containerManager.AddWater(a.Id, 10);
            AssertAmount(21.5, 21.5, 21.5, 21.5, 19);

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
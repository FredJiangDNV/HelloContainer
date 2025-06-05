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
            var a = new Container();
            var b = new Container();
            var c = new Container();
            var d = new Container();

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
    }
}
